using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Identity;
using System.Text;
using Microsoft.Extensions.Configuration;
using AzureAutomation.Interfaces;
using AzureAutomation.Models;
using System.Collections.Generic;

namespace AzureAutomation.Functions
{
    public class example
    {
        private readonly IConfiguration configuration;
        private readonly IBlobStorageService blobStorageService;
        List<DataProcessingResponse> allShipments = new();

        public example(IBlobStorageService blobStorageService)
        {
            this.blobStorageService = blobStorageService;
        }

        [FunctionName("exampleseaaz01")]
        public async Task<List<string>> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";//"UseDevelopmentStorage=true";

            ShipmentDataTransformation shipmentDataTransformation = new(log, blobStorageService);

            BlobServiceClient serviceClient = new(connectionString);

            BlobContainerClient blobContainerClient = blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientstorageaccountname");

            var allShipmentDatajsons = blobContainerClient.GetBlobsAsync(); //AsyncPageable<BlobItem>

            List<string> finalOutput = new();


            await foreach (var shipmentData in allShipmentDatajsons)
            {
                try
                {
                    var currentShipmentName = shipmentData.Name;

                    BlobClient blobClient = blobContainerClient.GetBlobClient(currentShipmentName);

                    if (await blobClient.ExistsAsync())
                    {
                        using (MemoryStream stream = new())
                        {
                            await blobClient.DownloadToAsync(stream);

                            string blobContent = Encoding.UTF8.GetString(stream.ToArray());

                            DataProcessingResponse jsonData = JsonConvert.DeserializeObject<DataProcessingResponse>(blobContent);

                            allShipments.Add(jsonData);

                            //allContent.Add(Convert.ToString(blobContents));

                            //await blobClient.DeleteAsync();
                        }
                    }
                }


                catch (Exception ex)
                {
                    log.LogInformation($"There was an issue: {ex.Message} and {ex.StackTrace}");
                }

            }

            finalOutput = shipmentDataTransformation.RouteJsonData(allShipments);

            BlobClient blobClient1 = blobContainerClient.GetBlobClient("output.txt");

            var abc = blobStorageService.UploadBlobContent(blobClient1, finalOutput[0]);

            return finalOutput;

        }
    }
}
