using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Application.WebApps
{

    public class AzureConfiguration
    {
        public string AzureStorageConnectionString { get; set; }
        public string AzureFileStorageReference { get; set; }
        public string AzureVisionAPISubscriptionKey { get; set; }
        public string AzureVisionAPIEndPoint { get; set; }
        public string AzureCosmosDBConnectionString { get; set; }
        public string AzureCosmonDBPrimaryKey1 { get; set; }
    }

    public sealed class ConfigInit
    {
        public static AzureConfiguration getConfiguration()
        {
            XmlDocument xmldc = new XmlDocument();
            xmldc.Load("http://handwrittenservice.azurewebsites.net/AzureConfig/AzureSettings.xml");
            AzureConfiguration configInit = new AzureConfiguration()
            {
                AzureStorageConnectionString = xmldc.GetElementsByTagName("AzureStorageConnectionString")[0].InnerText,
                AzureFileStorageReference = xmldc.GetElementsByTagName("AzureFileStorageReference")[0].InnerText,
                AzureVisionAPISubscriptionKey = xmldc.GetElementsByTagName("AzureVisionAPISubscriptionKey")[0].InnerText,
                AzureVisionAPIEndPoint = xmldc.GetElementsByTagName("AzureVisionAPIEndPoint")[0].InnerText,
                AzureCosmosDBConnectionString = xmldc.GetElementsByTagName("AzureCosmosDBConnectionString")[0].InnerText,
                AzureCosmonDBPrimaryKey1 = xmldc.GetElementsByTagName("AzureCosmonDBPrimaryKey1")[0].InnerText,
            };
            return configInit;
        }
    }
}