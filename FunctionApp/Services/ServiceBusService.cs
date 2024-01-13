namespace AzureAutomation.Interfaces.Services
{
    using System.Text;
    using Azure.Messaging.ServiceBus;

    public class ServicebusService : IServiceBusService
    {
        public ServiceBusClient GetIntegrationServiceBusClient(string serviceBusConnection)
        {
            return new ServiceBusClient("");
        }

        public ServiceBusSender GetIntegrationServiceBusSender(ServiceBusClient serviceBusClient, string serviceBusTopicName)
        {
            return serviceBusClient.CreateSender(serviceBusTopicName);
        }

        public void SendOutputLocToLa(ServiceBusSender serviceBusSender, string topicContent, string subsLabel, string sessiosnID, string clientTrackingID)
        {
            ServiceBusMessage serviceBusMessage = new(topicContent)
            {
                ContentType = "application/json",
                SessionId = sessiosnID
            };
            serviceBusMessage.ApplicationProperties["x-ms-client-tracking-id"] = clientTrackingID;
            serviceBusSender.SendMessageAsync(serviceBusMessage);
        }
    }
}