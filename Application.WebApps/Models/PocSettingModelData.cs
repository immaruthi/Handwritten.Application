using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApps.Models
{
    public class PocSettingModelData
    {
        public int id { get; set; }
        
        public string AzureConnectionString { get; set; }
        public string FileStorageReference { get; set; }

        public string CosmosDBConnectionString { get; set; }
        public string cosmosDBPrimaryKey { get; set; }

        public string VisionAPIConnectionString { get; set; }
        public string VisionAPISubscriptionKey { get; set; }


    }
}