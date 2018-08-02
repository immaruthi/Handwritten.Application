using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApps.Models
{
    public class ExtractHandwrittenText
    {
        [JsonProperty(PropertyName = "id")]
        public string OCR_Id { get; set; }

        [JsonProperty(PropertyName = "sourcename")]
        public string SourceName { get; set; }

        [JsonProperty(PropertyName = "documentURL")]
        public string DocURL { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}