namespace AzureAutomation.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using System.Collections.Generic;
    using Azure.Messaging.ServiceBus;

    public interface IServiceBusService
    {
        ServiceBusClient GetIntegrationServiceBusClient(string serviceBusConnection);

        ServiceBusSender GetIntegrationServiceBusSender(ServiceBusClient serviceBusClient, string serviceBusTopicName);

        void SendOutputLocToLa(ServiceBusSender serviceBusSender, string topicContent, string subsLabel, string sessiosnID, string clientTrackingID);

    }
}