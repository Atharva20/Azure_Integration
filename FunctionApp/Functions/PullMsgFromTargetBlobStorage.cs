namespace AzureIntegration.Functions
{
    using System;
    using System.Text;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;
    using Azure.Storage.Blobs;
    using AzureIntegration.Interfaces;
    using AzureIntegration.Models;
    using AzureIntegration.Transformation;
    using AzureIntegration.Globals;
    using Azure.Storage.Blobs.Models;
    using Azure;
    using Azure.Messaging.ServiceBus;
    using AzureIntegration.Logger;

    public class PullMsgFromTargetBlobStorage
    {
        private readonly ILoggerWrapper log;
        private readonly IConfiguration configuration;
        private readonly IBlobStorageService blobStorageService;
        private readonly IServiceBusService serviceBusService;
        public PullMsgFromTargetBlobStorage(IConfiguration configuration, IBlobStorageService blobStorageService, IServiceBusService serviceBusService, ILoggerWrapper log)
        {
            this.log = log;
            this.configuration = configuration;
            this.blobStorageService = blobStorageService;
            this.serviceBusService = serviceBusService;
        }

        /// <summary>
        /// The Function extracts every 15 mins to read the blobs in the client location, transform and then load the output in client location.
        /// </summary>
        /// <param name="myTimer"></param>
        [FunctionName("PullMsgFromTargetBlobStorage")]
        public async Task Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer)
        {
            try
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}.");

                string fullyQualifiedNamespace = configuration["servicebus_fullyqualified_namespace"];

                List<string> allShipmetContent = new();

                ShipmentDataTransformation shipmentDataTransformation = new();

                int outputShipments = 0;

                string blobServiceEndpoint = configuration["client_storage_account_url"];

                BlobServiceClient blobServiceClient = blobStorageService.ConnectToTargetStorageAccountUsingManagedIdentity(blobServiceEndpoint);

                BlobContainerClient inboundBlobContainerClient = blobStorageService.GetTargetBlobConatinerFromClientLocation(blobServiceClient, "inboundshipmetdata");

                BlobContainerClient outboundBlobContainerClient = this.blobStorageService.GetTargetBlobConatinerFromClientLocation(blobServiceClient, "outboundshipmetdata");

                string blobDirectoryLoc = $"transformed-shipment-csv/{DateTime.Now:yyyy/MM/dd/HH/mm}";

                allShipmetContent = await blobStorageService.GetAllBlobsContent(inboundBlobContainerClient);

                if (allShipmetContent.Count > 0)
                {
                    foreach (var currentShipment in allShipmetContent)
                    {
                        DataProcessingResponse jsonData = JsonConvert.DeserializeObject<DataProcessingResponse>(currentShipment);

                        OutputStrucutre outputCsv = shipmentDataTransformation.TransformJsonToCsv(jsonData);

                        string outputBlobName = $"{blobDirectoryLoc}/{outputCsv.OriginFacilityID}.txt";

                        blobStorageService.AppendContentToBlob(outboundBlobContainerClient, outputBlobName, outputCsv.ResponseContent);

                        outputShipments++;

                        log.LogInformation($"The currentShipment Data is successfully uploaded to the location.");
                    }
                }
                else
                {
                    log.LogWarning("There was no data for the current trigger.");
                }

                if (outputShipments > 0)
                {

                    ServiceBusClient serviceBusClient = serviceBusService.ConnectToTargetServiceBusUsingManagedIdentity(fullyQualifiedNamespace);

                    ServiceBusSender serviceBusSender = serviceBusService.GetServiceBusSender(serviceBusClient, $"{Globals.SB_TOPIC}");

                    serviceBusService.SendMsgToTopicSubs(serviceBusSender, "outbound_subs", blobDirectoryLoc);

                    log.LogInformation($"The processed shipment Data location is successfully sent to the client.");

                }
            }
            catch (Exception ex)
            {
                log.LogError($"There was an issue: {ex.Message} and {ex.StackTrace}");
                throw;
            }
        }
    }
}
