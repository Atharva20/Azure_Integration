// using System;
// using System.Threading.Tasks;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using Azure.Identity;
// using Microsoft.WindowsAzure.Storage.Auth;
// using Microsoft.WindowsAzure.Storage;
// using Microsoft.WindowsAzure.Storage.Blob;
// using Azure.Core;
// using System.Collections.Generic;

// namespace AzureAutomation.Functions
// {
//     public static class PullMessage
//     {
//         [FunctionName("pullmessageseaaz01")]
//         public static async Task<List<string>> Run(
//             [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
//             ILogger log)
//         {
//             log.LogInformation("C# HTTP trigger function processed a request.");

//             string storageAccountName = "saazdevsea01";
//             List<string> outputContent = new();

//             // Create a ManagedIdentityCredential
//             var managedIdentityCredential = new ManagedIdentityCredential();

//             // Use the ManagedIdentityCredential to obtain a token
//             var token = managedIdentityCredential.GetToken(new TokenRequestContext(new[] { "https://storage.azure.com/.default" })).Token;

//             // Create a StorageCredentials instance using the token
//             var storageCredentials = new StorageCredentials(new Microsoft.WindowsAzure.Storage.Auth.TokenCredential(token));

//             // Create a CloudStorageAccount with the storage account name and the managed identity credentials
//             CloudStorageAccount storageAccount = new(storageCredentials, storageAccountName, endpointSuffix: null, useHttps: true);

//             // Create a CloudBlobClient using the CloudStorageAccount
//             CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

//             //try try

//             CloudBlobContainer cloudBlobContainer2= cloudBlobClient.GetContainerReference("atharvaContainer");

//             bool isContainerExists2 = await cloudBlobContainer2.ExistsAsync();

//             if (!isContainerExists2)
//             {
//                 await cloudBlobContainer2.CreateIfNotExistsAsync();
//             }

//             //tyr try khatam

//             CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("clientstorageaccount");

//             bool isContainerExists = await cloudBlobContainer.ExistsAsync();

//             if (!isContainerExists)
//             {
//                 await cloudBlobContainer.CreateIfNotExistsAsync();
//             }

//             List<IListBlobItem> listOfAllBlobs = new();

//             BlobContinuationToken blobContinuationToken = null;
//             do
//             {
//                 BlobResultSegment blobResultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(blobContinuationToken);
//                 listOfAllBlobs.AddRange(blobResultSegment.Results);
//                 blobContinuationToken = blobResultSegment.ContinuationToken;

//             } while (blobContinuationToken != null);

//             string output = string.Empty;

//             foreach (IListBlobItem currentBlob in listOfAllBlobs)
//             {
//                 try
//                 {
//                     CloudBlockBlob cloudBlockBlob = currentBlob as CloudBlockBlob;
//                     string currentBlobName = cloudBlockBlob.Name;
//                     string blobContent = await cloudBlockBlob.DownloadTextAsync();
//                     outputContent.Add(blobContent);                    
//                 }
//                 catch (Exception ex)
//                 {
//                     log.LogInformation($"The issue is beacuse of {ex.Message} and {ex.StackTrace}.");
//                 }
//             }

            


//             return outputContent;

//         }
//     }
// }
