using Application.WebApps.Models;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Application.WebApps.Services
{
    public class CosmosAPIService
    {
        public async Task<bool> UpdateDocumentCollection(UpdateRequestData updateRequestData,DocumentClient documentClient)
        {
            bool response = await Task.FromResult<bool>(true);
            try
            {
                var objCollection = new CosmosDocumentCollection()
                {
                    extracthandwrittenText = new ExtractHandwrittenText()
                    {
                        OCR_Id = updateRequestData.cosmosID,
                        SourceName = "File Storage",
                        Status = "Inserted"
                    },
                    text = new Text()
                    {
                        Id = "1",
                        OCR_Id = updateRequestData.cosmosID,
                        TextDetails = updateRequestData.imageText
                    },
                    textLocation = new TextLocation()
                    {
                        Id = "1",
                        LocationName = "sample",
                        OCR_Id = updateRequestData.cosmosID,
                        OtherDetails = "sample"
                    },
                    textDocumentType = new TextDocumentType()
                    {
                        Id = "1",
                        Details = updateRequestData.imageText,
                        OCR_Texttid = "sample"
                    },
                    docUserDetails = new DocUserDetails()
                    {
                        Id = "1",
                        OtherDetails = "POC SCAN",
                        UserName = "maruthi pallamalli"
                    }
                };

                var objResponse = await documentClient.UpsertDocumentAsync(UriFactory.CreateDocumentUri("CognitiveAPIDemoDataSource", "PocCollection", updateRequestData.cosmosID), objCollection);
            }
            catch(Exception ex)
            {
                response = await Task.FromResult<bool>(false);
            }
            return response;
        }

        public async Task<bool> CreateDocumentCollection(List<APIData> apiData,DocumentClient documentClient)
        {
            bool response = await Task.FromResult<bool>(true);

            try
            {
                foreach (var apidata in apiData)
                {
                    var objCollection = new CosmosDocumentCollection()
                    {
                        extracthandwrittenText = new ExtractHandwrittenText()
                        {
                            OCR_Id = apidata.OCRID,
                            DocURL = apidata.ImageUrl,
                            SourceName = "File Storage",
                            Status = "Inserted"
                        },
                        text = new Text()
                        {
                            Id = "1",
                            DocUserId = apidata.sid,
                            OCR_Id = apidata.OCRID,
                            TextDetails = apidata.imageText
                        },
                        textLocation = new TextLocation()
                        {
                            Id = "1",
                            LocationName = "sample",
                            OCR_Id = apidata.OCRID,
                            OtherDetails = "sample"
                        },
                        textDocumentType = new TextDocumentType()
                        {
                            Id = "1",
                            Details = apidata.imageText,
                            OCR_Texttid = "sample"
                        },
                        docUserDetails = new DocUserDetails()
                        {
                            Id = "1",
                            OtherDetails = "POC SCAN",
                            UserName = "maruthi pallamalli"
                        }
                    };

                    var insertDocumentResponse = await documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("CognitiveAPIDemoDataSource", "PocCollection"), objCollection);
                }

            }
            catch (Exception ex)
            {
                response = await Task.FromResult<bool>(false);
            }

            return response;
        }
    }
}