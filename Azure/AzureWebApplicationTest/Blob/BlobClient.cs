using Azure.Storage.Blobs;
using System;

namespace AzureWebApplicationTest.BLob
{
    public class BLobClient
    {
        private const string ConnectionString = "DefaultEndpointsProtocol=https; ...";

        public static void UploadBlob()
        {
            BlobContainerClient container = GetBlobContainerClient();
            string blobName = "docs-and-friends-selfie-stick";
            string fileName = "docs-and-friends-selfie-stick.png";
            BlobClient blobClient = container.GetBlobClient(blobName);
            blobClient.Upload(fileName, true);
        }

        public static void GetAllBlobs()
        {
            BlobContainerClient container = GetBlobContainerClient();

            var blobs = container.GetBlobs();
            foreach (var blob in blobs)
            {
                Console.WriteLine($"{blob.Name} --> Created On: {blob.Properties.CreatedOn:yyyy-MM-dd HH:mm:ss}  Size: {blob.Properties.ContentLength}");
            }
        }

        private static BlobContainerClient GetBlobContainerClient()
        {
            string containerName = "photos";
            BlobContainerClient container = new BlobContainerClient(ConnectionString, containerName);
            container.CreateIfNotExists();

            return container;
        }
    }
}
