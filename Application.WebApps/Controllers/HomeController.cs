using Application.WebApps.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Application.WebApps.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpGet]
        public async Task<ActionResult> DoFileUploads()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> DoFileUploads(HttpPostedFileBase[] files)
        {
            List<APIData> objrApiData = new List<APIData>();

            for (int i=0;i<files.Length;i++)
            {
                if (files[i].FileName.EndsWith(".jpg") || files[i].FileName.EndsWith(".png"))
                {
                    objrApiData.Add(new APIData()
                    {
                        imageData = getbyteFromInputStream(files[i].InputStream, files[i].ContentLength),
                        imageText = "sample",
                        ImageUrl = "http://www.pactera.com",
                        pid = i,
                        Error = "no Error",
                        Remarks = "Remarks"
                    });
                }
            }

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



        //public IEnumerable<DisplayData> getDisplayData(List<APIData> apidatas=null)
        //{
        //    DisplayData display = new DisplayData();

        //    //List<APIData> apidata = new List<APIData>();

        //    //apidata.Add(new APIData()
        //    //{
        //    //    pid=1,
        //    //    imageData = GetImageAsByteArray(@"C:\Users\Bhargavi\Desktop\birds\sample.jpg"),
        //    //    imageText = "Hi This is sample text"
        //    //});

        //    //apidata.Add(new APIData()
        //    //{
        //    //    pid = 2,
        //    //    imageData = GetImageAsByteArray(@"C:\Users\Bhargavi\Desktop\birds\sample.jpg"),
        //    //    imageText = "Hi This is sample text"
        //    //});

        //    //apidata.Add(new APIData()
        //    //{
        //    //    pid = 3,
        //    //    imageData = GetImageAsByteArray(@"C:\Users\Bhargavi\Desktop\birds\sample.jpg"),
        //    //    imageText = "Hi This is sample text"
        //    //});

        //    display.listApiData = apidatas;

        //    List<DisplayData> displayDatas = new List<DisplayData>();

        //    displayDatas.Add(display);

        //    return displayDatas;
        //}

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