// namespace AzureIntegration.UnitTests
// {
//     using Xunit;
//     using System;
//     using Microsoft.Extensions.Configuration;
//     using Microsoft.Extensions.Logging; 
//     using AzureIntegration.Interfaces;
//     using Moq;
//     using AzureIntegration.Functions;
//     using System.Threading.Tasks;
//     using Microsoft.Azure.WebJobs;
//     using Azure.Storage.Blobs;
//     using System.Collections.Generic;
//     using AzureIntegration.UnitTestHelper;
//     using AzureIntegration.Utility;
//     using Microsoft.AspNetCore.Mvc;
//     using AzureIntegration.Models;

//     public class GetShipmentsFromClientTest : UnitTestHelper
//     {
//         private readonly GetShipmentsFromClient getShipmentsFromClient;
//         private readonly Mock<ILogger<GetShipmentsFromClient>> mockLogger = new();
//         private readonly Mock<ILogger<GetShipmentsFromClientUtlity>> mockLoggerUtility = new();
//         private readonly Mock<IConfiguration> mockConfiguration = new();
//         private readonly Mock<IBlobStorageService> mockBlobStorageService = new();
//         private readonly Mock<IServiceBusService> mockServiceBusService = new();

//         public GetShipmentsFromClientTest()
//         {
//             getShipmentsFromClient = new GetShipmentsFromClient(mockConfiguration.Object,mockBlobStorageService.Object,mockServiceBusService.Object,mockLogger.Object,mockLoggerUtility.Object);
//         }

//         [Fact]
//         public async Task GetShipmentsFromClient_ShipmentExists_ReturnsCsvOutput() //methodname_scenario_expectation
//         {
//             //var getShipmentsFromClientUtlity = new Mock<GetShipmentsFromClientUtlity>(mockConfiguration.Object, mockBlobStorageService.Object, mockServiceBusService.Object, mockLoggerUtility.Object);
//             //Arrange
//             mockConfiguration.Setup(x => x[It.Is<string>(s => s == "servicebus_fullyqualified_namespace")]).Returns("servicebus_conectionstring");
//             mockConfiguration.Setup(x => x[It.Is<string>(s => s == "client_storage_account_url")]).Returns("https://dummyblob.blob.core.windows.net");

//             //Assert
//             mockBlobStorageService.Setup(x => x.GetAllBlobsContent(It.IsAny<BlobContainerClient>()))
//             .ReturnsAsync(new List<Tuple<string, string>>
//             {
//                 new("shipmet1.txt", GetShipmentData1()),
//                 new("shipment2.txt", GetShipmentData2()),
//             });

//             //getShipmentsFromClientUtlity.Setup(x=>x.UploadPerShipmentCsvDataToClientLoc(It.IsAny<List<Tuple<string, string>>>(), It.IsAny<string>(),It.IsAny<BlobContainerClient>())).Returns(2);
//             //Act
//             var result = await getShipmentsFromClient.Run(new TimerInfo(null, null));

//             Assert.NotNull(result);
//         }

//         [Fact]
//         public async Task GetShipmentsFromClient_ShipmentDoesNotExists_ReturnsBadRequest() //methodname_scenario_expectation
//         {
//             //Arrange
//             mockConfiguration.Setup(x => x[It.Is<string>(s => s == "servicebus_fullyqualified_namespace")]).Returns("servicebus_conectionstring");
//             mockConfiguration.Setup(x => x[It.Is<string>(s => s == "client_storage_account_url")]).Returns("https://dummyblob.blob.core.windows.net");

//             //Act
//             mockBlobStorageService.Setup(x => x.GetAllBlobsContent(It.IsAny<BlobContainerClient>())).ReturnsAsync(new List<Tuple<string, string>> { });
//             var result = await getShipmentsFromClient.Run(new TimerInfo(null, null));

//             //Assert
//             Assert.IsType<BadRequestObjectResult>(result);

//         }

//         [Fact]
//         public async Task GetShipmentsFromClient_ShipmentExists_ReturnsShipmentDetailsResponse() //methodname_scenario_expectation
//         {
//             //Arrange
//             string currentDateTime = $"{DateTime.Now:yyyy/MM/dd/HH/mm}";
//             mockConfiguration.Setup(x => x[It.Is<string>(s => s == "servicebus_fullyqualified_namespace")]).Returns("servicebus_conectionstring");
//             mockConfiguration.Setup(x => x[It.Is<string>(s => s == "client_storage_account_url")]).Returns("https://dummyblob.blob.core.windows.net");

//             //Act
//             mockBlobStorageService.Setup(x => x.GetAllBlobsContent(It.IsAny<BlobContainerClient>()))
//             .ReturnsAsync(new List<Tuple<string, string>>
//             {
//                 new("SampleShipment1.txt", GetShipmentData1()),
//                 new("SampleShipment2.txt", GetShipmentData2()),
//                 new("dummyData.txt", "dummyData"),
//             });

//             var result = await getShipmentsFromClient.Run(new TimerInfo(null, null));

//             //Assert
//             Assert.Contains($"transformed-shipment-csv/{currentDateTime}", result.OutputTransformedLoc);
//             Assert.Contains("3", Convert.ToString(result.TotalShipmentsReceieved));
//             Assert.Contains("2", Convert.ToString(result.TotalShipmentsTransformed));
//         }
//     }
// }
