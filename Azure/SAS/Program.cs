using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using System;
using System.IO;
using System.Threading.Tasks;
namespace SAS
{
    class Program
    {
        static string accountname = "demostore20900";
        static string accountkey = "";
        static string containerName = "data";
        static string blobname = "sample.txt";

        static async Task Main(string[] args)
        {
            Uri sharedacecss = GenerateSAS();
            Console.WriteLine(sharedacecss);
            ReadBlob(sharedacecss).Wait();
            Console.ReadKey();
        }

        static Uri GenerateSAS()
        {
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerName,
                BlobName = blobname,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(5)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var storageSharedKeyCredential = new StorageSharedKeyCredential(accountname, accountkey);

            string sasToken = sasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();

            // Build the full URI to the Blob SAS
            UriBuilder w_fullUri = new UriBuilder()
            {
                Scheme = "https",
                Host = string.Format("{0}.blob.core.windows.net", accountname),
                Path = string.Format("{0}/{1}", containerName, blobname),
                Query = sasToken
            };

            return w_fullUri.Uri;
        }

        static async Task ReadBlob(Uri p_SASUri)
        {
            BlobClient blobClient = new BlobClient(p_SASUri, null);

            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
            using (StreamReader reader = new StreamReader(blobDownloadInfo.Content, true))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
