using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApps.Models
{
    public class Text
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "OCR_ID")]
        public string OCR_Id { get; set; }

        [JsonProperty(PropertyName = "textDetails")]
        public string TextDetails { get; set; }

        [JsonProperty(PropertyName = "documentUserID")]
        public string DocUserId { get; set; }
    }
}