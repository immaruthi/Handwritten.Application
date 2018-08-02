using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApps.Models
{
    public class TextLocation
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "OCR_ID")]
        public string OCR_Id { get; set; }

        [JsonProperty(PropertyName = "locationName")]
        public string LocationName { get; set; }

        [JsonProperty(PropertyName = "otherDetails")]
        public string OtherDetails { get; set; }
    }
}