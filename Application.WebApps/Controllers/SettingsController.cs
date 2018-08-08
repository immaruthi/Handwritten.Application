using Application.WebApps.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Application.WebApps.Controllers
{
    public class SettingsController : Controller
    {

        XmlDocument xmldc = new XmlDocument();

        public IEnumerable<PocSettingModelData> getData()
        {
            var obj = new List<PocSettingModelData>();

            obj.Add(new PocSettingModelData
            {
                id = 1,
                AzureConnectionString = "DefaultEndpointsProtocol=https;AccountName=ocrextracthandwrittentx;AccountKey=pOHbwrr3hGc7Q0msNk7rgEIDtuKq54YofeoJeOOicm05TLYtnNPvnaxnU+ruC+oZesmwp2qoYzge+pvUq1ihvA==;EndpointSuffix=core.windows.net",
                CosmosDBConnectionString = "https://cosmosdbfor.documents.azure.com:443/",
                cosmosDBPrimaryKey = "vY20dhewGFN7ARZA88DvJ77rdXwBhUc2IINoD9FzCmDMyD78hbd6U484TV2EuqJnnZd2vJyhudPT0Q6ezKcJpQ==",
                FileStorageReference = "ocrextracthandwrittentx",
                VisionAPIConnectionString = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/recognizeText",
                VisionAPISubscriptionKey = "908a6575a1da43a9aa736f8bf8dd5124"
            });
            
            return obj;
        }

        // GET: Settings
        public ActionResult Index()
        {

            DataSet ds = new DataSet();

            

            return View(getData());
        }

        // GET: Settings/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Settings/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Settings/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //// GET: Settings/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Settings/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
