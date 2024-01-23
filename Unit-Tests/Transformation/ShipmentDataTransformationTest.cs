namespace AzureIntegration.UnitTests
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
    using AzureIntegration.UnitTestHelper;
    using AzureIntegration.Logger;

    public class ShipmentDataTransformationTest : UnitTestHelper
    {

        public ShipmentDataTransformationTest()
        {
        }

        [Fact]
        public void ShipmentDataTransformationTest_ClientDataExists_logsInfoSuccessfully() //methodname_scenario_expectation
        {
        }

    }
}
