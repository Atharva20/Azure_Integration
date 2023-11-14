// namespace AzureAutomation.Functions
// {
//     using System;
//     using System.Text;
//     using Microsoft.Azure.WebJobs;
//     using Microsoft.Extensions.Logging;
//     using Azure.Storage.Blobs;
//     using System.Threading.Tasks;
//     using Microsoft.Extensions.Configuration;
//     using AzureAutomation.Interfaces;
//     using System.Collections.Generic;
//     using System.IO;
//     using AzureAutomation.Models;
//     using Newtonsoft.Json;
//     using Azure;
//     using Azure.Storage.Blobs.Models;

//     public class PullMsgFromClinet
//     {
//         private readonly IConfiguration configuration;
//         private readonly IBlobStorageService blobStorageService;
//         public PullMsgFromClinet(IConfiguration configuration, IBlobStorageService blobStorageService)
//         {
//             this.configuration = configuration;
//             this.blobStorageService = blobStorageService;
//         }

//         [FunctionName("PullMsgFromTargetBlobStorage")]
//         public async Task Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer, ILogger log)
//         {
//             try
//             {
//                 log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

//                 List<DataProcessingResponse> allShipments = new();

//                 BlobContainerClient blobContainerClient = null;

//                 BlobServiceClient serviceClient = null;

//                 List<string> dataProcessingInput = new();
//                 List<string> finalOutput = new();

//                 string blobServiceEndpoint = configuration["blob-storage-endpoint-url"];

//                 serviceClient = blobStorageService.ConnectToTargetStorageAccountUsingManagedIdentity(blobServiceEndpoint);

//                 ShipmentDataTransformation shipmentDataTransformation = new(log, blobStorageService);

//                 blobContainerClient = blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientStorageAccountName");

//                 var allShipmentDatajsons = blobContainerClient.GetBlobsAsync(); //AsyncPageable<BlobItem>

//                 await foreach (var shipmentData in allShipmentDatajsons)
//                 {
//                     try
//                     {
//                         var currentShipmentName = shipmentData.Name;

//                         BlobClient blobClient = blobContainerClient.GetBlobClient(currentShipmentName);

//                         if (await blobClient.ExistsAsync())
//                         {
//                             using (MemoryStream stream = new())
//                             {
//                                 await blobClient.DownloadToAsync(stream);

//                                 string blobContent = Encoding.UTF8.GetString(stream.ToArray());

//                                 DataProcessingResponse jsonData = JsonConvert.DeserializeObject<DataProcessingResponse>(blobContent);

//                                 allShipments.Add(jsonData);

//                                 await blobClient.DeleteAsync();
//                             }
//                         }
//                     }
//                     catch (Exception ex)
//                     {
//                         log.LogInformation($"There was an issue: {ex.Message} and {ex.StackTrace}");
//                     }

//                 }
//                 finalOutput = shipmentDataTransformation.RouteJsonData(allShipments);

//             }
//             catch (Exception ex)
//             {
//                 log.LogInformation($"There was an issue: {ex.Message} and {ex.StackTrace}");
//             }
//         }
//     }
// }
