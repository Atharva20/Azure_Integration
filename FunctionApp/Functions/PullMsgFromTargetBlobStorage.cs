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
    using Azure;
    using Azure.Storage.Blobs.Models;
    using AzureAutomation.Transformation;
    using AzureIntegration.Globals;
    using Azure.Messaging.ServiceBus;
    using Azure.Identity;


    public class PullMsgFromTargetBlobStorage
    {
        private readonly IConfiguration configuration;
        private readonly IBlobStorageService blobStorageService;
        public PullMsgFromTargetBlobStorage(IConfiguration configuration, IBlobStorageService blobStorageService)
        {
            this.configuration = configuration;
            this.blobStorageService = blobStorageService;
        }

        [FunctionName("PullMsgFromTargetBlobStorage")]
        public async Task Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

                string outputBlobName = string.Empty;

                BlobContainerClient blobContainerClient = null;

                BlobContainerClient blobContainerClient2 = null;

                BlobServiceClient serviceClient = null;

                ShipmentDataTransformation shipmentDataTransformation = new(this.blobStorageService);

                string blobServiceEndpoint = configuration["client_storage_account_url"];

                serviceClient = blobStorageService.ConnectToTargetStorageAccountUsingManagedIdentity(blobServiceEndpoint);

                blobContainerClient = blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "inboundshipmetdata");

                blobContainerClient2 = this.blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "outboundshipmetdata");

                string blobDirectoryLoc = $"{DateTime.Now:yyyy/MM/dd/HH/mm}";

                var allShipmentDatajsons = blobContainerClient.GetBlobsAsync(); //AsyncPageable<BlobItem>

                await foreach (var shipmentData in allShipmentDatajsons)
                {
                    try
                    {
                        BlobClient blobClient = blobContainerClient.GetBlobClient(shipmentData.Name);

                        using (MemoryStream stream = new())
                        {
                            await blobClient.DownloadToAsync(stream);

                            string blobContent = Encoding.UTF8.GetString(stream.ToArray());

                            DataProcessingResponse jsonData = JsonConvert.DeserializeObject<DataProcessingResponse>(blobContent);

                            List<string> outputCsv = shipmentDataTransformation.TransformJsonToCsv(jsonData);

                            outputBlobName = $"transformed-shipment-csv/{blobDirectoryLoc}/{outputCsv[0]}.txt";

                            blobStorageService.AppendContentToBlob(blobContainerClient2, outputBlobName, outputCsv[1]);

                            await blobClient.DeleteAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.LogInformation($"There was an issue: {ex.Message} and {ex.StackTrace}");
                    }
                }

                var fullyQualifiedNamespace = configuration["servicebus_fullyqualified_namespace"];//"servicebus1234512345.servicebus.windows.net"; // servicebus1234512345.servicebus.windows.net

                var topicName = Globals.SB_TOPIC; // https://saazdevsea01.blob.core.windows.net/clientstorageaccount

                var topicSubsName = Globals.SB_SUBSCRIPTION;

                var messageBody = $"Hello, Service Bus!";

                var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));

                var client = new ServiceBusClient(fullyQualifiedNamespace, new ManagedIdentityCredential());

                var sender = client.CreateSender($"{topicName}/{topicSubsName}");

                ServiceBusMessage messages = new(Encoding.UTF8.GetBytes($"The message is sent successfully"))
                {
                    Subject = "outbound_subs"
                };

                await sender.SendMessageAsync(messages);

            }
            catch (Exception ex)
            {
                log.LogInformation($"There was an issue: {ex.Message} and {ex.StackTrace}");
            }
        }
    }
}
