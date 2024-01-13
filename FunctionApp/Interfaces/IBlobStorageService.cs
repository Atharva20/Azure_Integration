namespace AzureAutomation.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using System.Collections.Generic;

    public interface IBlobStorageService
    {
        BlobServiceClient ConnectToTargetStorageAccountUsingManagedIdentity(string endpointUrl);

        BlobContainerClient GetTargetBlobConatinerFromClientLocation(BlobServiceClient blobServiceClient, string containerName);

        Task<List<string>> AppendBlobDataToList(BlobContainerClient blobContainerClient);

        void UploadDataToBLob(BlobClient blobClient, string content);

        Task UploadBlobContent(BlobClient blobClient, string blobContent);

        void AppendContentToBlob(BlobContainerClient blobContainerClient, string blobName, string blobData);
    }
}