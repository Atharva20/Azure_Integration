namespace AzureAutomation.Interfaces.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Azure.Identity;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using System.IO;
    using System.Text;
    using Azure.Storage.Blobs.Specialized;

    public class BlobStorageService : IBlobStorageService
    {
        public BlobServiceClient ConnectToTargetStorageAccountUsingManagedIdentity(string endpointUrl)
        {
            ChainedTokenCredential chainedTokenCredential = new(new ManagedIdentityCredential(), new VisualStudioCodeCredential());
            BlobServiceClient serviceClient = new(new Uri(endpointUrl), chainedTokenCredential);
            return serviceClient;
        }

        public BlobContainerClient GetTargetBlobConatinerFromClientLocation(BlobServiceClient blobServiceClient, string containerName)
        {
            BlobContainerClient targetblobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return targetblobContainerClient;
        }

        public async Task<string> ReadBlobContent(BlobContainerClient blobContainerClient, string blobName)
        {
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
            using Stream contentStream = blobDownloadInfo.Content;
            using StreamReader reader = new(contentStream);
            string content = await reader.ReadToEndAsync();
            return content;
        }

        public async Task UploadBlobContent(BlobClient blobClient, string blobContent)
        {
            byte[] blobContentBytes = Encoding.UTF8.GetBytes(blobContent);
            using (MemoryStream stream = new(blobContentBytes))
            {
                await blobClient.UploadAsync(stream);
            }
        }

        public async Task<List<string>> AppendBlobDataToList(BlobContainerClient blobContainerClient)
        {
            List<string> blobDataList = new();

            foreach (BlobItem blobItem in blobContainerClient.GetBlobs())
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobItem.Name);

                BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
                using (Stream contentStream = blobDownloadInfo.Content)
                using (StreamReader reader = new(contentStream))
                {
                    string content = await reader.ReadToEndAsync();
                    blobDataList.Add(content);

                    await blobClient.DeleteAsync();
                }
            }
            return blobDataList;
        }

        public void UploadDataToBLob(BlobClient blobClient, string content)
        {
            // blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
            // // Get a reference to the blob
            // BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = "text/plain" });
        }

        public async void AppendContentToBlob(BlobContainerClient blobContainerClient, string blobName, string blobData)
        {
            AppendBlobClient appendBlobClient = blobContainerClient.GetAppendBlobClient(blobName);
            await appendBlobClient.CreateIfNotExistsAsync();
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(blobData);
            using (MemoryStream stream = new(dataBytes))
            {
                await appendBlobClient.AppendBlockAsync(stream);
            }
        }

    }
}
