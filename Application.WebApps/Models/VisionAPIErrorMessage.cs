using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApps.Models
{
    
        public class Error
        {
            public string code { get; set; }
            public string message { get; set; }
        }

        public class VisionAPIErrorMessage
        {
            public Error error { get; set; }
        }
    
}