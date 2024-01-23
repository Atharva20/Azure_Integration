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

    public class PullMsgFromTargetBlobStorageTest : UnitTestHelper
    {
        private readonly Mock<ILoggerWrapper> mockLogger;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly Mock<IBlobStorageService> mockBlobStorageService;
        private readonly Mock<IServiceBusService> mockServiceBusService;

        public PullMsgFromTargetBlobStorageTest()
        {
            this.mockLogger = new Mock<ILoggerWrapper>();
            this.mockConfiguration = new Mock<IConfiguration>();
            this.mockBlobStorageService = new Mock<IBlobStorageService>();
            this.mockServiceBusService = new Mock<IServiceBusService>();
        }

        [Fact]
        public async Task PullMsgFromTargetBlobStorage_ClientDataExists_logsInfoSuccessfully() //methodname_scenario_expectation
        {

            var function = new PullMsgFromTargetBlobStorage(mockConfiguration.Object, mockBlobStorageService.Object, mockServiceBusService.Object, mockLogger.Object);

            var capturedLogs = new List<string>();

            mockConfiguration.Setup(x => x[It.Is<string>(s => s == "servicebus_fullyqualified_namespace")]).Returns("servicebus_conectionstring");
            mockConfiguration.Setup(x => x[It.Is<string>(s => s == "client_storage_account_url")]).Returns("https://dummyblob.blob.core.windows.net");

            mockBlobStorageService.Setup(x => x.GetAllBlobsContent(It.IsAny<BlobContainerClient>()))
            .ReturnsAsync(new List<string>
            {
                GetShipmentData1(),
                GetShipmentData2(),
            });

            mockLogger.Setup(x => x.LogInformation(It.IsAny<string>())).Callback<string>(message => capturedLogs.Add(message));

            await function.Run(new TimerInfo(null, null, false));

            Assert.Contains("The currentShipment Data is successfully uploaded to the location.", capturedLogs);
        }

        [Fact]
        public async Task PullMsgFromTargetBlobStorage_ClientDataDoesNotExists_logsWarningSuccessfully() //methodname_scenario_expectation
        {

            var function = new PullMsgFromTargetBlobStorage(mockConfiguration.Object, mockBlobStorageService.Object, mockServiceBusService.Object, mockLogger.Object);

            var capturedLogs = new List<string>();

            mockConfiguration.Setup(x => x[It.Is<string>(s => s == "servicebus_fullyqualified_namespace")]).Returns("servicebus_conectionstring");
            mockConfiguration.Setup(x => x[It.Is<string>(s => s == "client_storage_account_url")]).Returns("https://dummyblob.blob.core.windows.net");

            mockBlobStorageService.Setup(x => x.GetAllBlobsContent(It.IsAny<BlobContainerClient>()))
            .ReturnsAsync(new List<string> { });

            mockLogger.Setup(x => x.LogWarning(It.IsAny<string>())).Callback<string>(message => capturedLogs.Add(message));

            await function.Run(new TimerInfo(null, null, false));

            Assert.Contains("There was no data for the current trigger.", capturedLogs);
        }

        [Fact]
        public async Task PullMsgFromTargetBlobStorage_ClientDataExists_logsServiceBusMsgSuccessfully() //methodname_scenario_expectation
        {

            var function = new PullMsgFromTargetBlobStorage(mockConfiguration.Object, mockBlobStorageService.Object, mockServiceBusService.Object, mockLogger.Object);

            var capturedLogs = new List<string>();

            mockConfiguration.Setup(x => x[It.Is<string>(s => s == "servicebus_fullyqualified_namespace")]).Returns("servicebus_conectionstring");
            mockConfiguration.Setup(x => x[It.Is<string>(s => s == "client_storage_account_url")]).Returns("https://dummyblob.blob.core.windows.net");

            mockBlobStorageService.Setup(x => x.GetAllBlobsContent(It.IsAny<BlobContainerClient>()))
            .ReturnsAsync(new List<string>
            {
                GetShipmentData1(),
                GetShipmentData2(),
            });

            mockLogger.Setup(x => x.LogInformation(It.IsAny<string>())).Callback<string>(message => capturedLogs.Add(message));

            await function.Run(new TimerInfo(null, null, false));

            Assert.Contains("The processed shipment Data location is successfully sent to the client.", capturedLogs);
        }

        [Fact]
        public async Task PullMsgFromTargetBlobStorage_ClientDataExists_Exception() //methodName_scenario_expectation
        {

            await Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                var function = new PullMsgFromTargetBlobStorage(mockConfiguration.Object, mockBlobStorageService.Object, mockServiceBusService.Object, mockLogger.Object);

                var capturedLogs = new List<string>();

                mockBlobStorageService.Setup(x => x.GetAllBlobsContent(It.IsAny<BlobContainerClient>()))
                .ReturnsAsync(new List<string>
                {
                    "incorrectData","faultyData"
                });

                await function.Run(new TimerInfo(null, null, false));
            });
        }
    }
}
