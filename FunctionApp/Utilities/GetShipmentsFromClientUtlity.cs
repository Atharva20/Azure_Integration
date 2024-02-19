namespace AzureIntegration.Utility
{
    using System;
    using System.Collections.Generic;
    using AzureIntegration.Models;
    using Microsoft.Extensions.Configuration;
    using Azure.Storage.Blobs;
    using AzureIntegration.Transformation;
    using AzureIntegration.Globals;
    using Azure.Messaging.ServiceBus;
    using AzureIntegration.Interfaces;
    using Microsoft.Extensions.Logging;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;

    /// <summary>
    /// Converts the shipment jsons to csv format.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetShipmentsFromClientUtlity
    {
        private readonly ILogger<GetShipmentsFromClientUtlity> logger;
        private readonly IConfiguration configuration;
        private readonly IBlobStorageService blobStorageService;
        private readonly IServiceBusService serviceBusService;
        public GetShipmentsFromClientUtlity(IConfiguration configuration, IBlobStorageService blobStorageService, IServiceBusService serviceBusService, ILogger<GetShipmentsFromClientUtlity> logger)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.blobStorageService = blobStorageService;
            this.serviceBusService = serviceBusService;
        }

        /// <summary>
        /// The method iteratess over the list of blob contetnt and uploads the transformed output to client location.
        /// </summary>
        /// <param name="allShipmetContent">Contains list of shipments from target location.</param>
        /// <param name="clientCsvShipmentLoc">The blob path to upload the transformed data.</param>
        /// <param name="outboundBlobContainerClient">The client required to connect to the client storage accounts conntainer.</param>
        /// <returns></returns>
        public int UploadPerShipmentCsvDataToClientLoc(List<Tuple<string, string>> allShipmetContent, string clientCsvShipmentPath, BlobContainerClient outboundBlobContainerClient)
        {
            int transformedCsvShipments = 0;

            if (allShipmetContent.Count > 0)
            {
                foreach (var currentShipment in allShipmetContent)
                {
                    try
                    {

                        OutputStrucutre outputCsvShipment = ShipmentDataTransformation.TransformJsonToCsv(currentShipment.Item2);

                        string outputBlobName = $"{clientCsvShipmentPath}/{outputCsvShipment.OriginFacilityID}.txt";

                        blobStorageService.AppendContentToBlob(outboundBlobContainerClient, outputBlobName, outputCsvShipment.ResponseContent);

                        logger.LogInformation($"The shipment data for {currentShipment.Item1} is successfully uploaded to the location.");

                        transformedCsvShipments++;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"The shipment for the blob {currentShipment.Item1} was not processed due to the error stating : {ex.Message} and {ex.StackTrace}.");
                    }
                }
            }
            else
            {
                logger.LogWarning("There was no data for the current trigger.");
                throw new Exception("Empty request body.");
            }

            return transformedCsvShipments;
        }

        /// <summary>
        /// Sends the the directory location for transformed shipments to client.
        /// </summary>
        /// <param name="transformedCsvShipments">Provides the count for the transformed shipments.</param>
        /// <param name="clientCsvShipmentLoc">The blob path where the transformed shipments are stored.</param>
        public string SendShipmentCsvLocToClient(int shipmentsReceieved, int transformedCsvShipments, string clientCsvShipmentLoc)
        {

            ShipmentResponse shipmentResponse = new()
            {
                TotalShipmentsReceieved = shipmentsReceieved,
                TotalShipmentsTransformed = transformedCsvShipments,
                OutputTransformedLoc = clientCsvShipmentLoc
            };

            string sbContent = JsonSerializer.Serialize(shipmentResponse);           

            string fullyQualifiedNamespace = configuration["servicebus_fullyqualified_namespace"];

            ServiceBusClient serviceBusClient = serviceBusService.ConnectToTargetServiceBusUsingManagedIdentity(fullyQualifiedNamespace);

            ServiceBusSender serviceBusSender = serviceBusService.GetServiceBusSender(serviceBusClient, $"{Globals.SB_TOPIC}");

            serviceBusService.SendMsgToTopicSubs(serviceBusSender, "outbound_subs", sbContent);

            logger.LogInformation($"The directory location{clientCsvShipmentLoc} containing {transformedCsvShipments} transformed Data is successfully shared to the client.");

            return sbContent;

        }
    }
}
