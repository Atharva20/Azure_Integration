// using System.Threading.Tasks;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Configuration;
// using AzureIntegration.Interfaces;
// using Azure.Storage.Blobs;
// using System.Collections.Generic;
// using Azure.Messaging.ServiceBus;
// using System.Text;
// using Azure.Identity;
// using System;
// using AzureIntegration.Globals;
// using System.IO;

// namespace Company.Function
// {
//     public class ManagedIdentityPractise
//     {

//         private readonly IConfiguration configuration;
//         private readonly IBlobStorageService blobStorageService;
//         public ManagedIdentityPractise(IConfiguration configuration, IBlobStorageService blobStorageService)
//         {
//             this.configuration = configuration;
//             this.blobStorageService = blobStorageService;
//         }

//         [FunctionName("ManagedIdentityPractise")]
//         public async Task<string> Run(
//             [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
//             ILogger log)
//         {

//             List<string> asd = new();

//             string status1 = "Failure1";

//             string status2 = "Failure2";

//             try
//             {


//                 BlobServiceClient serviceClient = blobStorageService.ConnectToTargetStorageAccountUsingManagedIdentity(configuration["client_storage_account_url"]);

//                 BlobContainerClient blobContainerClient = blobStorageService.GetTargetBlobConatinerFromClientLocation(serviceClient, "clientstorageaccount");

//                 var allShipmentDatajsons = blobContainerClient.GetBlobsAsync(); //AsyncPageable<BlobItem>

//                 await foreach (var shipmentData in allShipmentDatajsons)
//                 {

//                     asd.Add(shipmentData.Name);

//                     status1 = "success1";

//                 }

//             }
//             catch (Exception ex)
//             {
//                 status1 = $"{ex.StackTrace} and {ex.Message} and {ex} hence the message failed";

//             }

//             try
//             {

//                 var fullyQualifiedNamespace = "servicebus1234512345.servicebus.windows.net"; // servicebus1234512345.servicebus.windows.net

//                 var topicName = Globals.SB_TOPIC; // https://saazdevsea01.blob.core.windows.net/clientstorageaccount

//                 var topicSubsName = Globals.SB_SUBSCRIPTION;

//                 var messageBody = $"{asd.ToArray()} Hello, Service Bus!";

//                 var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));

//                 var client = new ServiceBusClient(fullyQualifiedNamespace, new ManagedIdentityCredential());

//                 var sender = client.CreateSender($"{topicName}/{topicSubsName}");

//                 ServiceBusMessage messages = new(Encoding.UTF8.GetBytes($"The message {asd.ToArray()} and {asd.Count} is sent successfully"))
//                 {
//                     Subject = "outbound_subs"
//                 };

//                 await sender.SendMessageAsync(messages);

//                 status2 = "success2";

//             }
//             catch (Exception ex)
//             {
//                 status2 = $"{ex.StackTrace} and {ex.Message} and {ex} hence the message failed";

//             }

//             string output = $"{status1} and {status2} are the final results";

//             return output;
//         }
//     }
// }