namespace AzureAutomation.Functions
{
    using System;
    using System.Text;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Azure.Storage.Blobs;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using AzureAutomation.Interfaces;
    using System.Collections.Generic;
    using System.IO;
    using AzureAutomation.Models;
    using Newtonsoft.Json;
    using AzureAutomation.Transformation;
    using AzureIntegration.Globals;

    public class PullMsgFromTargetBlobStorage
    {
        private readonly IConfiguration configuration;
        private readonly IBlobStorageService blobStorageService;
        private readonly IServiceBusService serviceBusService;
        public PullMsgFromTargetBlobStorage(IConfiguration configuration, IBlobStorageService blobStorageService, IServiceBusService serviceBusService)
        {
            this.configuration = configuration;
            this.blobStorageService = blobStorageService;
            this.serviceBusService = serviceBusService;
        }

        /// <summary>
        /// The Function extracts every 15 mins to read the blobs in the client location, transform and then load the output in client location.
        /// </summary>
        /// <param name="log">logs the status of every step.</param>
        /// <returns></returns>
        [FunctionName("PullMsgFromTargetBlobStorage")]
        public async Task Run([TimerTrigger("0 */15 * * * *")] ILogger log)
        {
            try
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

                string fullyQualifiedNamespace = configuration["servicebus_fullyqualified_namespace"];

                BlobServiceClient serviceClient = null;

                ShipmentDataTransformation shipmentDataTransformation = new();

                string blobServiceEndpoint = configuration["client_storage_account_url"];

                serviceClient = blobStorageService.ConnectToTargetStorageAccountUsingManagedIdentity(blobServiceEndpoint);

                BlobContainerClient inboundBlobContainerClient = blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "inboundshipmetdata");

                BlobContainerClient outboundBlobContainerClient = this.blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "outboundshipmetdata");

                string blobDirectoryLoc = $"transformed-shipment-csv/{DateTime.Now:yyyy/MM/dd/HH/mm}";

                var allShipmentDatajsons = inboundBlobContainerClient.GetBlobsAsync(); //AsyncPageable<BlobItem>

                await foreach (var shipmentData in allShipmentDatajsons)
                {
                    try
                    {
                        BlobClient blobClient = inboundBlobContainerClient.GetBlobClient(shipmentData.Name);

                        using (MemoryStream stream = new())
                        {
                            await blobClient.DownloadToAsync(stream);

                            string blobContent = Encoding.UTF8.GetString(stream.ToArray());

                            DataProcessingResponse jsonData = JsonConvert.DeserializeObject<DataProcessingResponse>(blobContent);

                            List<string> outputCsv = shipmentDataTransformation.TransformJsonToCsv(jsonData);

                            string outputBlobName = $"{blobDirectoryLoc}/{outputCsv[0]}.txt";

                            blobStorageService.AppendContentToBlob(outboundBlobContainerClient, outputBlobName, outputCsv[1]);

                            await blobClient.DeleteAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.LogInformation($"There was an issue: {ex.Message} and {ex.StackTrace}");
                    }
                }

                var client = serviceBusService.ConnectToTargetServiceBusUsingManagedIdentity(fullyQualifiedNamespace);

                var abcdef = serviceBusService.GetServiceBusSender(client, $"{Globals.SB_TOPIC}");

                serviceBusService.SendMsgToTopicSubs(abcdef, "outbound_subs", blobDirectoryLoc);

            }
            catch (Exception ex)
            {
                log.LogInformation($"There was an issue: {ex.Message} and {ex.StackTrace}");
            }
        }
    }
}
