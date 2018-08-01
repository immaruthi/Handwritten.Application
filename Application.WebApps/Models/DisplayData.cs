using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApps.Models
{
    //public class DisplayData
    //{
    //    public List<APIData> listApiData { get; set; }
    //}


    public class APIData
    {
        public int pid { get; set; }
        public byte[] imageData { get; set; }
        public string imageText { get; set; }
        public string ImageUrl { get; set; }
        public string Error { get; set; }
        public string Remarks { get; set; }
    }
}