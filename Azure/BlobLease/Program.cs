using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace BlobLease
{
    class Program
    {
        static string connstring = "";

        static void Main(string[] args)
        {
            WorkwithBlob().GetAwaiter().GetResult();
            Console.ReadKey();

        }

        private static async Task WorkwithBlob()
        {
            CloudStorageAccount l_storageAccount;
            if (CloudStorageAccount.TryParse(connstring, out l_storageAccount))
            {
                CloudBlobClient l_cloudBlobClient = l_storageAccount.CreateCloudBlobClient();

                CloudBlobContainer l_cloudBlobContainer = l_cloudBlobClient.GetContainerReference("demo");

                var l_blockBlob = l_cloudBlobContainer.GetBlockBlobReference("audio.log");
                TimeSpan? l_leaseTime = TimeSpan.FromSeconds(60);
                string leaseID = await l_blockBlob.AcquireLeaseAsync(l_leaseTime, null);
                await l_blockBlob.FetchAttributesAsync();
                Console.WriteLine("The lease state is " + l_blockBlob.Properties.LeaseState);
                Console.WriteLine("The lease duration is " + l_blockBlob.Properties.LeaseDuration);
            }
        }
    }
}
