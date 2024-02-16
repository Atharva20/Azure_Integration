// using System;
// using System.Threading.Tasks;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.AspNetCore.Http;
// using Azure.Storage.Blobs;
// using AzureIntegration.Interfaces;
// using System.Collections.Generic;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using AzureIntegration.Models;
// using AzureIntegration.Utility;
// using Microsoft.Extensions.Logging;

// namespace AzureIntegration.Functions
// {
//     public class LocalMicrosoftEmulatorPractise
//     {
//         private readonly ILogger<LocalMicrosoftEmulatorPractise> _logger;
//         private readonly ILogger<GetShipmentsFromClientUtlity> _logger2;
//         private readonly IConfiguration configuration;
//         private readonly IBlobStorageService blobStorageService;
//         private readonly IServiceBusService serviceBusService;


//         public LocalMicrosoftEmulatorPractise(IConfiguration configuration, IBlobStorageService blobStorageService, IServiceBusService serviceBusService, ILogger<LocalMicrosoftEmulatorPractise> logger, ILogger<GetShipmentsFromClientUtlity> logger2)
//         {
//             this._logger = logger;
//             this._logger2 = logger2;
//             this.configuration = configuration;
//             this.blobStorageService = blobStorageService;
//             this.serviceBusService = serviceBusService;
//         }

//         [FunctionName("LocalMicrosoftEmulatorPractise")]
//         public async Task<IActionResult> Run(
//             [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
//         {
//             _logger.LogInformation("C# HTTP trigger function processed a request.");

//             // string outputBlobName = string.Empty;

//             // List<DataProcessingResponse> allShipments = new();

//             // List<string> allShipmetContent = new();

//             // string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";//"UseDevelopmentStorage=true";

//             // ShipmentDataTransformation shipmentDataTransformation = new();

//             // BlobServiceClient serviceClient = new(connectionString);

//             // BlobContainerClient inboundBlobContainerClient = this.blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientstorageaccountname");

//             // BlobContainerClient outboundBlobContainerClient = this.blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientoutputaccountname");

//             // //var allShipmentDatajsons = blobContainerClient.GetBlobsAsync(); //AsyncPageable<BlobItem>

//             // string blobDirectoryLoc = $"{DateTime.Now:yyyy/MM/dd/HH/mm}";

//             // await blobStorageService.GetAllBlobsContent(inboundBlobContainerClient, allShipmetContent);  //inboundBlobContainerClient.GetBlobsAsync(); //AsyncPageable<BlobItem>

//             // if (allShipmetContent.Count > 0)
//             // {
//             //     foreach (var currentShipment in allShipmetContent)
//             //     {
//             //         DataProcessingResponse jsonData = JsonConvert.DeserializeObject<DataProcessingResponse>(currentShipment);

//             //         OutputStrucutre outputCsv = shipmentDataTransformation.TransformJsonToCsv(jsonData);

//             //         outputBlobName = $"{blobDirectoryLoc}/{outputCsv.OriginFacilityID}.txt";

//             //         blobStorageService.AppendContentToBlob(outboundBlobContainerClient, outputBlobName, outputCsv.ResponseContent);
//             //     }
//             // }
//             // else
//             // {
//             //     log.LogInformation("Theere was no data for the current run.");
//             // }

//             // new

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
//             //  ........................................... NEW NEW NEW NEW NEW NEW ......................................................
//             List<Tuple<string, string>> allShipmetContent = new();

//             ShipmentResponse outputResponse = null;

//             try
//             {
//                 string currentDateTime = $"{DateTime.Now:yyyy/MM/dd/HH/mm}";

//                 _logger.LogInformation($"C# Timer trigger function executed at: {currentDateTime}.");

//                 GetShipmentsFromClientUtlity getShipmentsFromClientUtlity = new(configuration, blobStorageService, serviceBusService, _logger2);

//                 //string blobServiceEndpoint = configuration["client_storage_account_url"];

//                 //BlobServiceClient blobServiceClient = blobStorageService.ConnectToTargetStorageAccountUsingManagedIdentity(blobServiceEndpoint);

//                 //string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";//"UseDevelopmentStorage=true";

//                 BlobServiceClient serviceClient = new(configuration["AzureWebJobsStorage"]);

//                 BlobContainerClient inboundBlobContainerClient = blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientstorageaccountname");

//                 BlobContainerClient outboundBlobContainerClient = blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientoutputaccountname");

//                 string clientCsvShipmentLoc = $"transformed-shipment-csv/{currentDateTime}";

//                 allShipmetContent = await blobStorageService.GetAllBlobsContent(inboundBlobContainerClient);

//                 int transformedCsvShipments = getShipmentsFromClientUtlity.UploadPerShipmentCsvDataToClientLoc(allShipmetContent, clientCsvShipmentLoc, outboundBlobContainerClient);

//                 if (transformedCsvShipments > 0)
//                 {
//                     ShipmentResponse shipmentResponse = new()
//                     {
//                         TotalShipmentsReceieved = allShipmetContent.Count,
//                         TotalShipmentsTransformed = transformedCsvShipments,
//                         OutputTransformedLoc = clientCsvShipmentLoc
//                     };
//                     outputResponse = shipmentResponse;
//                     //getShipmentsFromClientUtlity.SendShipmentCsvLocToClient(transformedCsvShipments, clientCsvShipmentLoc);
//                 }

//                 return new OkObjectResult(outputResponse);

//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"The blob path containing transformed shipments cannot be shared because of {ex.Message} and {ex.StackTrace}");
//                 return new BadRequestObjectResult(ex.Message);
//             }
//         }
//     }
// }
