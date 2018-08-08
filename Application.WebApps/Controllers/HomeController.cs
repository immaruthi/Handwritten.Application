using Application.WebApps.Models;
using Application.WebApps.Services;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Application.WebApps.Controllers
{
    public class HomeController : Controller
    {
        private byte[] bytesFromImage;
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpGet]
        public async Task<ActionResult> DoFileUploads()
        {
            //try
            //{
            //    //"http://handwrittenservice.azurewebsites.net/AzureConfig/AzureSettings.xml"

            //    XmlDocument doc = new XmlDocument();
            //    doc.Load(@"AzureConfig\AzureSettings.xml");

            //    // get a list of nodes - in this case, I'm selecting all <AID> nodes under
            //    // the <GroupAIDs> node - change to suit your needs
            //    XmlNodeList aNodes = doc.SelectNodes("/AzureApplication/Configuration/sample");

            //    // loop through all AID nodes
            //    foreach (XmlNode aNode in aNodes)
            //    {
            //        // grab the "id" attribute
            //        string idAttribute = aNode.ChildNodes[0].Value;

            //        // check if that attribute even exists...
            //        if (!string.IsNullOrEmpty(idAttribute))
            //        {
            //            idAttribute = "515";
            //        }
            //    }

            //    doc.GetElementsByTagName("sample")[0].InnerText = "sample x";

            //    // save the XmlDocument back to disk
            //    doc.Save(@"AzureConfig\AzureSettings.xml");
            //}
            //catch(Exception ex)
            //{

            //}
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> DoFileUploads(HttpPostedFileBase[] files)
        {
            // Step 1: Intialize the Azure Configuration form azure settings config
            var azureconfiguration = ConfigInit.getConfiguration();

            // Step 2: Intialize the Azure DataLake Storage
            var storageAccountConnectionString_File = CloudStorageAccount.Parse(azureconfiguration.AzureStorageConnectionString);
            CloudFileClient cloudFileClient = storageAccountConnectionString_File.CreateCloudFileClient();
            CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(azureconfiguration.AzureFileStorageReference);
            CloudFileDirectory rootDir = cloudFileShare.GetRootDirectoryReference();
            CloudFile fileSas = null;

            // Step 3: Intailize the API Client
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", azureconfiguration.AzureVisionAPISubscriptionKey);
            HttpResponseMessage response;

            // Step 4: Intailize the Cosmos DB
            var cosmosDBResponse = new CosmosAPIService();
            DocumentClient documentClient = new DocumentClient(new Uri(azureconfiguration.AzureCosmosDBConnectionString), azureconfiguration.AzureCosmonDBPrimaryKey1);
            var createDataBaseResponse = await documentClient.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database { Id = "CognitiveAPIDemoDataSource" });
            var createDataBaseCollectionResponse = await documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("CognitiveAPIDemoDataSource"), new DocumentCollection { Id = "PocCollection" }, new RequestOptions { OfferThroughput = 1000 });

            List<APIData> objrApiData = new List<APIData>();

            for (int i=0;i<files.Length;i++)
            {
                if (files[i].FileName.EndsWith(".jpg") || files[i].FileName.EndsWith(".png"))
                {
                    bytesFromImage = getbyteFromInputStream(files[i].InputStream, files[i].ContentLength);

                    if (cloudFileShare.Exists())
                    {
                        fileSas = rootDir.GetFileReference(Path.GetFileName(files[i].FileName));
                        fileSas.UploadFromStream(new MemoryStream(bytesFromImage));
                    }


                    // Extracting Image :
                    using (ByteArrayContent content = new ByteArrayContent(bytesFromImage))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response = await client.PostAsync(azureconfiguration.AzureVisionAPIEndPoint + "?" + "mode=Handwritten", content);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        string operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();
                        string contentString;
                        int res = 0;
                        do
                        {
                            System.Threading.Thread.Sleep(1000);
                            response = await client.GetAsync(operationLocation);
                            contentString = await response.Content.ReadAsStringAsync();
                            ++res;
                        }
                        while (res < 10 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1);

                        if (res == 10 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1)
                        {
                            //return null;
                        }

                        var rootobject = JsonConvert.DeserializeObject<RootObject>(contentString);
                        StringBuilder builder = new StringBuilder();
                        for (int s = 0; s < rootobject.recognitionResult.lines.Count; s++)
                        {
                            builder.Append(rootobject.recognitionResult.lines[s].text.ToString());
                        }
                        objrApiData.Add(new APIData()
                        {
                            OCRID = Guid.NewGuid().ToString(),
                            imageData = bytesFromImage,
                            imageText = builder.ToString(),
                            ImageUrl = Path.GetFileName(files[i].FileName),//fileSas.Uri.AbsoluteUri.ToString(),
                            pid = i,
                            sid = "text" + i,
                            Error = "no Error",
                            Remarks = "No Remarks Found"
                        });
                    }
                    else
                    {
                        string errorString = await response.Content.ReadAsStringAsync();
                        var errorObject = JsonConvert.DeserializeObject<VisionAPIErrorMessage>(errorString);

                        objrApiData.Add(new APIData()
                        {
                            OCRID= Guid.NewGuid().ToString(),
                            imageData = bytesFromImage,
                            imageText = errorObject.error.message.ToString(),
                            ImageUrl = Path.GetFileName(files[i].FileName),//fileSas.Uri.AbsoluteUri.ToString(),
                            pid = i,
                            sid = "text" + i,
                            Error = errorObject.error.message.ToString(),
                            Remarks = "Image scanning Failed"
                        });
                    }
                }
            }
            var dbProcessingResponse = await cosmosDBResponse.CreateDocumentCollection(objrApiData, documentClient);
            TempData["objrApiData"] = objrApiData;
            return RedirectToAction("ShowResults");
        }

        [HttpGet]
        public async Task<ActionResult> GetSettings()
        {
            return RedirectToAction("Index", "Settings", new { area = "" });
        }

        private byte[] getbyteFromInputStream(Stream stream,int slength)
        {
            byte[] byteresponse = null;
            using (var binaryReader = new BinaryReader(stream))
            {
                byteresponse = binaryReader.ReadBytes(slength);
            }
            return byteresponse;
        }

        [HttpGet]
        public async Task<ActionResult> ShowResults()
        {
            var apidatas = TempData["objrApiData"] as List<APIData>;
            return View(apidatas);
        }

        [HttpPost]
        public async Task<ActionResult> GetGridData(UpdateRequestData UserModel)
        {
            var azureconfiguration = ConfigInit.getConfiguration();

            var cosmosDBResponse = new CosmosAPIService();
            DocumentClient documentClient = new DocumentClient(new Uri(azureconfiguration.AzureCosmosDBConnectionString), azureconfiguration.AzureCosmonDBPrimaryKey1);
            var createDataBaseResponse = await documentClient.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database { Id = "CognitiveAPIDemoDataSource" });
            var createDataBaseCollectionResponse = await documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("CognitiveAPIDemoDataSource"), new DocumentCollection { Id = "PocCollection" }, new RequestOptions { OfferThroughput = 1000 });

            var documentResponse = await cosmosDBResponse.UpdateDocumentCollection(UserModel, documentClient);

            if(!documentResponse)
            {
                return Json(new { cosmosID = UserModel.cosmosID, imageText = UserModel.imageText, status = "Fail" });
            }

            return Json(new { cosmosID = UserModel.cosmosID, imageText = UserModel.imageText, status = "Success" });
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}