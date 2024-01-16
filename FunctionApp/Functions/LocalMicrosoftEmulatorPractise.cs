// using System;
// using System.IO;
// using System.Threading.Tasks;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;
// using Azure.Storage.Blobs;
// using System.Text;
// using AzureAutomation.Interfaces;
// using AzureAutomation.Models;
// using System.Collections.Generic;
// using Azure.Storage.Blobs.Models;
// using Microsoft.AspNetCore.Mvc;
// using AzureAutomation.Transformation;

// namespace AzureAutomation.Functions
// {
//     public class LocalMicrosoftEmulatorPractise
//     {
//         //private readonly IConfiguration configuration;
//         private readonly IBlobStorageService blobStorageService;
//         private readonly IServiceBusService serviceBusService;


//         public LocalMicrosoftEmulatorPractise(IBlobStorageService _blobStorageService, IServiceBusService _serviceBusService)
//         {
//             this.blobStorageService = _blobStorageService;
//             this.serviceBusService = _serviceBusService;
//         }

//         [FunctionName("LocalMicrosoftEmulatorPractise")]
//         public async Task<IActionResult> Run(
//             [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
//             ILogger log)
//         {
//             log.LogInformation("C# HTTP trigger function processed a request.");
            
//             string outputBlobName = string.Empty;

//             List<DataProcessingResponse> allShipments = new();

//             string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";//"UseDevelopmentStorage=true";

//             ShipmentDataTransformation shipmentDataTransformation = new(this.blobStorageService);

//             BlobServiceClient serviceClient = new(connectionString);

//             BlobContainerClient blobContainerClient = this.blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientstorageaccountname");

//             BlobContainerClient blobContainerClient2 = this.blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientoutputaccountname");
            
//             var allShipmentDatajsons = blobContainerClient.GetBlobsAsync(); //AsyncPageable<BlobItem>

//             string blobDirectoryLoc = $"{DateTime.Now:yyyy/MM/dd/HH/mm}";

//             await foreach (BlobItem shipmentData in allShipmentDatajsons)
//             {
//                 try
//                 {
//                     BlobClient blobClient = blobContainerClient.GetBlobClient(shipmentData.Name);

//                     using (MemoryStream stream = new())
//                     {
//                         await blobClient.DownloadToAsync(stream);

//                         string blobContent = Encoding.UTF8.GetString(stream.ToArray());

//                         DataProcessingResponse jsonData = JsonConvert.DeserializeObject<DataProcessingResponse>(blobContent);

//                         List<string> outputCsv = shipmentDataTransformation.TransformJsonToCsv(jsonData);

//                         outputBlobName = $"transformed-shipment-csv/{blobDirectoryLoc}/{outputCsv[0]}.txt";

//                         blobStorageService.AppendContentToBlob(blobContainerClient2, outputBlobName, outputCsv[1]);

//                         await blobClient.DeleteAsync();
//                     }

//                 }
//                 catch (Exception ex)
//                 {
//                     log.LogInformation($"There was an issue for the blob {shipmentData.Name} with error: {ex.Message} and {ex.StackTrace}");
//                 }
//             }
//             return new OkObjectResult(outputBlobName);
//         }
//     }
// }