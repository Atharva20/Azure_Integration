using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Identity;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;

namespace AzureAutomation.Functions
{
    public static class TryOut
    {
        [FunctionName("TryOut")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";//"UseDevelopmentStorage=true";



            // Container and blob names
            string containerName = "shipconfirm";
            //string blobName = "emucontainer/atharva.txt";

            // Content to upload
            //string contentToUpload = "Hello, Atharva!";

            // Create a BlobServiceClient
            BlobServiceClient serviceClient = new BlobServiceClient(connectionString);

            // Get a reference to the container
            BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(containerName);

            // Create the container if it doesn't exist
            await containerClient.CreateIfNotExistsAsync();
            List<string> allContent = new();

            var abc = containerClient.GetBlobsAsync();

            await foreach (var a in abc)
            {
                var name = a.Name;
                BlobClient blobClient1 = containerClient.GetBlobClient(name);
                if (await blobClient1.ExistsAsync())
                {
                    // Download the blob content to a MemoryStream
                    var abcd = blobClient1.DownloadContent();
                    using (MemoryStream stream = new())
                    {
                        await blobClient1.DownloadToAsync(stream);

                        string blobContents = System.Text.Encoding.UTF8.GetString(stream.ToArray());

                        //allContent.Add(Convert.ToString(blobContents));

                        await blobClient1.DeleteAsync();

                    }
                }
            }

            // // Get a reference to the blob
            // BlobClient blobClient = containerClient.GetBlobClient(blobName);


            // if (await blobClient.ExistsAsync())
            // {
            //     // Download the blob content to a MemoryStream
            //     var abcd = blobClient.DownloadContent();
            //     using (MemoryStream stream = new MemoryStream())
            //     {
            //         await blobClient.DownloadToAsync(stream);

            //         // Do something with the content in the MemoryStream (e.g., read it as a string)
            //         string blobContents = System.Text.Encoding.UTF8.GetString(stream.ToArray());

            //         Console.WriteLine("Blob content:");
            //         Console.WriteLine(blobContents);
            //     }
            // }

            //Upload the blob content from the string
            // using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(contentToUpload)))
            // {
            //     await blobClient.UploadAsync(stream, true);
            // }

            // var blobContent = await blobClient.OpenReadAsync();

            // Console.WriteLine("Blob upload complete.");

        }
    }
}
