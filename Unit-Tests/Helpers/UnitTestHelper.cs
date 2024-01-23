namespace AzureIntegration.UnitTestHelper
{
    using Xunit;
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using AzureIntegration.Interfaces;
    using Moq;
    using AzureIntegration.Functions;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Azure.Storage.Blobs;
    using Azure;
    using Azure.Storage.Blobs.Models;
    using System.Collections.Generic;
    using System.IO;

    public class UnitTestHelper
    {
        public static string GetShipmentData1()
        {
            string localFilePath = @"./Data/SampleShipment1.txt";
            string shipmentData1 = File.ReadAllText(localFilePath);
            return shipmentData1;
        }

        public static string GetShipmentData2()
        {
            string localFilePath = @"./Data/SampleShipment2.txt";
            string shipmentData2 = File.ReadAllText(localFilePath);
            return shipmentData2;
        }
    }



}