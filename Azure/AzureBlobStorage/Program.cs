using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobStorage
{
    class Program
    {
        static string storageconnstring = "";
        static string containerName = "data";
        static string filename = "sample.txt";
        static string filepath = "C:\\Work\\sample.txt";
        static string downloadpath = "C:\\Work\\sample.txt";
        static async Task Main(string[] args)
        {
            Container().Wait();
            CreateBlob().Wait();
            GetBlobs().Wait();
            GetBlob().Wait();

            GetProperties();
            SetMetadata();
            GetMetadata();

            Console.WriteLine("Complete");
            Console.ReadKey();
        }

        static async Task Container()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageconnstring);

            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
        }

        static async Task CreateBlob()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageconnstring);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(filename);

            using FileStream uploadFileStream = File.OpenRead(filepath);

            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();
        }

        static async Task GetBlobs()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageconnstring);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
            }

        }

        static async Task GetBlob()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageconnstring);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blob = containerClient.GetBlobClient(filename);

            BlobDownloadInfo blobdata = await blob.DownloadAsync();

            using (FileStream downloadFileStream = File.OpenWrite(downloadpath))
            {
                await blobdata.Content.CopyToAsync(downloadFileStream);
                downloadFileStream.Close();
            }

            // Read the new file
            using (FileStream downloadFileStream = File.OpenRead(downloadpath))
            {
                using var strreader = new StreamReader(downloadFileStream);
                string line;
                while ((line = strreader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }

        }

        static void GetProperties()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageconnstring);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blob = containerClient.GetBlobClient(filename);

            BlobProperties properties = blob.GetProperties();
            Console.WriteLine("The Access tier of the blob is {0}", properties.AccessTier);
            Console.WriteLine("The Content Length of the blob is {0}", properties.ContentLength);
        }

        static void GetMetadata()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageconnstring);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blob = containerClient.GetBlobClient(filename);

            BlobProperties properties = blob.GetProperties();

            foreach (var metadata in properties.Metadata)
            {
                Console.WriteLine(metadata.Key.ToString());
                Console.WriteLine(metadata.Value.ToString());
            }
        }

        static void SetMetadata()
        {
            string p_key = "ApplicationType";
            string p_value = "Ecommerce";

            BlobServiceClient blobServiceClient = new BlobServiceClient(storageconnstring);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blob = containerClient.GetBlobClient(filename);

            IDictionary<string, string> obj = new Dictionary<string, string>();
            obj.Add(p_key, p_value);
            blob.SetMetadata(obj);



        }
    }
}
