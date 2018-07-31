using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Application.WebApps.Models
{
    public class ExtractDataRequestModel
    {
        public string referenceFileName { get; set; }
        public Stream fileStream { get; set; }
    }
}