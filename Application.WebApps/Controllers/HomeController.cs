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

            IEnumerable<ExtractDataRequestModel> extractDataRequestModels = files.Select(x => new ExtractDataRequestModel { referenceFileName = Path.GetFileName(x.FileName), fileStream = x.InputStream }); 

            return RedirectToAction("ShowResults");
            
        }

        [HttpGet]
        public async Task<ActionResult> GetSettings()
        {
            return RedirectToAction("Index", "Settings", new { area = "" });
        }

        [HttpGet]
        public async Task<ActionResult> ShowResults()
        {
            return View(getDisplayData());
        }



        public IEnumerable<DisplayData> getDisplayData(List<APIData> apidatas=null)
        {
            DisplayData display = new DisplayData();

            List<APIData> apidata = new List<APIData>();

            //apidata.Add(new APIData()
            //{
            //    pid=1,
            //    imageData = GetImageAsByteArray(@"C:\Users\Bhargavi\Desktop\birds\sample.jpg"),
            //    imageText = "Hi This is sample text"
            //});

            //apidata.Add(new APIData()
            //{
            //    pid = 2,
            //    imageData = GetImageAsByteArray(@"C:\Users\Bhargavi\Desktop\birds\sample.jpg"),
            //    imageText = "Hi This is sample text"
            //});

            //apidata.Add(new APIData()
            //{
            //    pid = 3,
            //    imageData = GetImageAsByteArray(@"C:\Users\Bhargavi\Desktop\birds\sample.jpg"),
            //    imageText = "Hi This is sample text"
            //});

            display.listApiData = apidata;

            List<DisplayData> displayDatas = new List<DisplayData>();

            displayDatas.Add(display);

            return displayDatas;
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