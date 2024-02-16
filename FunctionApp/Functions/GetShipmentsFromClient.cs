namespace AzureIntegration.Functions
{
    using System;
    using Microsoft.Azure.WebJobs;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using Azure.Storage.Blobs;
    using AzureIntegration.Interfaces;
    using AzureIntegration.Models;
    using AzureIntegration.Utility;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Diagnostics.CodeAnalysis;


    /// <summary>
    /// Fetches the shipment jsons and returns the transformed csv shipments directory path.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetShipmentsFromClient
    {
        private readonly ILogger<GetShipmentsFromClient> _logger;
        private readonly ILogger<GetShipmentsFromClientUtlity> _loggerUtility;
        private readonly IConfiguration configuration;
        private readonly IBlobStorageService blobStorageService;
        private readonly IServiceBusService serviceBusService;
        public GetShipmentsFromClient(IConfiguration configuration, IBlobStorageService blobStorageService, IServiceBusService serviceBusService, ILogger<GetShipmentsFromClient> logger, ILogger<GetShipmentsFromClientUtlity> loggerUtility)
        {
            this._logger = logger;
            this._loggerUtility = loggerUtility;
            this.configuration = configuration;
            this.blobStorageService = blobStorageService;
            this.serviceBusService = serviceBusService;
        }

        /// <summary>
        /// The Function extracts every 15 mins to read the blobs in the client location, transform and then load the output in client location.
        /// </summary>
        /// <param name="myTimer"></param>
        [FunctionName("GetShipmentsFromClient")]
        public async Task Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer)
        {

            List<Tuple<string, string>> allShipmetContent = new();

            try
            {
                string currentDateTime = $"{DateTime.Now:yyyy/MM/dd/HH/mm}";

                _logger.LogInformation($"C# Timer trigger function executed at: {currentDateTime}.");
                
                string blobServiceEndpoint = configuration["client_storage_account_url"];

                GetShipmentsFromClientUtlity getShipmentsFromClientUtlity = new(configuration, blobStorageService, serviceBusService, _loggerUtility);

                BlobServiceClient blobServiceClient = blobStorageService.ConnectToTargetStorageAccountUsingManagedIdentity(blobServiceEndpoint);

                BlobContainerClient inboundBlobContainerClient = blobStorageService.GetTargetBlobConatinerFromClientLocation(blobServiceClient, "inputshipmetjsons");

                BlobContainerClient outboundBlobContainerClient = blobStorageService.GetTargetBlobConatinerFromClientLocation(blobServiceClient, "outputshipmetcsv");

                string clientCsvShipmentLoc = $"transformed-shipment-csv/{currentDateTime}";

                allShipmetContent = await blobStorageService.GetAllBlobsContent(inboundBlobContainerClient);

                int transformedCsvShipments = getShipmentsFromClientUtlity.UploadPerShipmentCsvDataToClientLoc(allShipmetContent, clientCsvShipmentLoc, outboundBlobContainerClient);

                if (transformedCsvShipments > 0)
                {
                    getShipmentsFromClientUtlity.SendShipmentCsvLocToClient(transformedCsvShipments, clientCsvShipmentLoc);
                    _logger.LogInformation($"The blob path containing transformed shipments {clientCsvShipmentLoc} has been shared to the client subscription.");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"The blob path containing transformed shipments cannot be shared because of {ex.Message} and {ex.StackTrace}");
            }
        }
    }
}
