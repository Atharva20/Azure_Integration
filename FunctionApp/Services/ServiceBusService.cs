namespace AzureAutomation.Interfaces.Services
{
    using System.Text;
    using Azure.Messaging.ServiceBus;
    using Azure.Identity;

    public class ServicebusService : IServiceBusService
    {
        public ServiceBusClient ConnectToTargetServiceBusUsingManagedIdentity(string fullyQualifiedNamespace)
        {
            return new ServiceBusClient(fullyQualifiedNamespace, new ManagedIdentityCredential());
        }

        public ServiceBusSender GetServiceBusSender(ServiceBusClient serviceBusClient, string serviceBusTopicName)
        {
            return serviceBusClient.CreateSender(serviceBusTopicName);
        }

        public async void SendMsgToTopicSubs(ServiceBusSender serviceBusSender, string label, string topicContent)
        {
            ServiceBusMessage serviceBusMessage = new(topicContent)
            {
                ContentType = "application/json",
                Subject = label
            };
            await serviceBusSender.SendMessageAsync(serviceBusMessage);
        }
    }
}