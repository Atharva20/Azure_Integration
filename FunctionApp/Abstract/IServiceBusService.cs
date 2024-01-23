namespace AzureIntegration.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using System.Collections.Generic;
    using Azure.Messaging.ServiceBus;

    public interface IServiceBusService
    {
        /// <summary>
        /// Establishes connection with the servicebus using managed identity to the namespace.
        /// </summary>
        /// <param name="fullyQualifiedNamespace">namespaces of the target servicebus(https://<servicebusname>.blob.core.windows.net).</param>
        /// <returns></returns>
        ServiceBusClient ConnectToTargetServiceBusUsingManagedIdentity(string fullyQualifiedNamespace);

        /// <summary>
        /// Returns the servicebus sender client.
        /// </summary>
        /// <param name="serviceBusClient">Client required to connect to the storage account.</param>
        /// <param name="serviceBusTopicName">The servicebus topic where the message is to be sent. </param>
        /// <returns></returns>
        ServiceBusSender GetServiceBusSender(ServiceBusClient serviceBusClient, string serviceBusTopicName);

        /// <summary>
        /// Sends the message to provided label of the servicebus topic
        /// </summary>
        /// <param name="serviceBusSender">Client required to connect to the storage account.</param>
        /// <param name="label">The label/subject value that is required to identify the subscription.</param>
        /// <param name="topicContent">The servicebus messsage to be sent to the label.</param>
        void SendMsgToTopicSubs(ServiceBusSender serviceBusSender, string label, string topicContent);
    }
}