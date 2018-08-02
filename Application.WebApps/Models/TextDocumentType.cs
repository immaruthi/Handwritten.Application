using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApps.Models
{
    public class TextDocumentType
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "OCR_TextID")]
        public string OCR_Texttid { get; set; }

        [JsonProperty(PropertyName = "details")]
        public string Details { get; set; }
    }
}