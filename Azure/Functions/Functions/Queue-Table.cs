using System.IO;
using Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Functions.Functions
{
    public static class Queue_Table
    {
        [FunctionName("QueueTable")]
        // The ICollector interface is to write multiple to the output
        public static void Run(
            [QueueTrigger("appqueue", Connection = "storage_connection")] JObject myQueueItem,
            [Table("Customer", Connection = "storage_connection")] ICollector<Customer> outputTable,
            [Queue("newqueue", Connection = "storage_connection")] ICollector<Customer> outputQueue,
            [Blob("data/{rand-guid}", FileAccess.Write, Connection = "storage_connection")] TextWriter blobOutput,
     ILogger log)
        {
            log.LogInformation("Adding Customer");
            Customer obj = new Customer();
            obj.PartitionKey = myQueueItem["Id"].ToString();
            obj.RowKey = myQueueItem["Quantity"].ToString();
            outputTable.Add(obj); // Use ICollector<T>
            outputQueue.Add(obj); // The output can be an object serializable as JSON, string, byte[] and CloudQueueMessage
            blobOutput.Write($"Partition Key {obj.PartitionKey}");
            // For blob, you have an output of Stream, string, CloudBlockBlob
        }
    }
}
