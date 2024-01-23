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
// using AzureIntegration.Interfaces;
// using AzureIntegration.Models;
// using System.Collections.Generic;
// using Azure.Storage.Blobs.Models;
// using Microsoft.AspNetCore.Mvc;
// using AzureIntegration.Transformation;

// namespace AzureIntegration.Functions
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
//         public async Task Run(
//             [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
//             ILogger log)
//         {
//             log.LogInformation("C# HTTP trigger function processed a request.");

//             string outputBlobName = string.Empty;

//             List<DataProcessingResponse> allShipments = new();

//             List<string> allShipmetContent = new();

//             string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";//"UseDevelopmentStorage=true";

//             ShipmentDataTransformation shipmentDataTransformation = new();

//             BlobServiceClient serviceClient = new(connectionString);

//             BlobContainerClient inboundBlobContainerClient = this.blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientstorageaccountname");

//             BlobContainerClient outboundBlobContainerClient = this.blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientoutputaccountname");

//             //var allShipmentDatajsons = blobContainerClient.GetBlobsAsync(); //AsyncPageable<BlobItem>

//             string blobDirectoryLoc = $"{DateTime.Now:yyyy/MM/dd/HH/mm}";

//             await blobStorageService.GetAllBlobsContent(inboundBlobContainerClient, allShipmetContent);  //inboundBlobContainerClient.GetBlobsAsync(); //AsyncPageable<BlobItem>

//             if (allShipmetContent.Count > 0)
//             {
//                 foreach (var currentShipment in allShipmetContent)
//                 {
//                     DataProcessingResponse jsonData = JsonConvert.DeserializeObject<DataProcessingResponse>(currentShipment);

//                     OutputStrucutre outputCsv = shipmentDataTransformation.TransformJsonToCsv(jsonData);

//                     outputBlobName = $"{blobDirectoryLoc}/{outputCsv.OriginFacilityID}.txt";

//                     blobStorageService.AppendContentToBlob(outboundBlobContainerClient, outputBlobName, outputCsv.ResponseContent);
//                 }
//             }
//             else
//             {
//                 log.LogInformation("Theere was no data for the current run.");
//             }

//             // await foreach (BlobItem shipmentData in allShipmentDatajsons)
//             // {
//             //     try
//             //     {
//             //         BlobClient blobClient = blobContainerClient.GetBlobClient(shipmentData.Name);

//             //         using (MemoryStream stream = new())
//             //         {
//             //             await blobClient.DownloadToAsync(stream);

//             //             string blobContent = Encoding.UTF8.GetString(stream.ToArray());

//             //             DataProcessingResponse jsonData = JsonConvert.DeserializeObject<DataProcessingResponse>(blobContent);

//             //             List<string> outputCsv = shipmentDataTransformation.TransformJsonToCsv(jsonData);

//             //             outputBlobName = $"transformed-shipment-csv/{blobDirectoryLoc}/{outputCsv[0]}.txt";

//             //             blobStorageService.AppendContentToBlob(blobContainerClient2, outputBlobName, outputCsv[1]);

//             //             await blobClient.DeleteAsync();
//             //         }

//             //     }
//             //     catch (Exception ex)
//             //     {
//             //         log.LogInformation($"There was an issue for the blob {shipmentData.Name} with error: {ex.Message} and {ex.StackTrace}");
//             //     }
//             // }
//         }
//     }
// }