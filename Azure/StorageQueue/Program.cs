using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Threading.Tasks;

namespace StorageQueue
{
    class Program
    {
        private static string queue_connection_string = "";
        private static string queue_name = "appqueue";
        static async void Main(string[] args)
        {
            await Send();
            await Recieve();
        }

        static async Task Send()
        {
            CloudQueue _queue = GetCloudQueue();

            for (int i = 1; i < 10; i++)
            {
                Order obj = new Order();
                CloudQueueMessage _message = new CloudQueueMessage(obj.ToString());
                await _queue.AddMessageAsync(_message);
            }

            Console.WriteLine("All messages sent");
            Console.ReadLine();
        }

        static async Task Recieve()
        {
            CloudQueue _queue = GetCloudQueue();

            await _queue.FetchAttributesAsync();
            int? _count = _queue.ApproximateMessageCount;

            for (int i = 0; i < _count; i++)
            {
                CloudQueueMessage _message = await _queue.GetMessageAsync();
                Console.WriteLine(_message.AsString);
                await _queue.DeleteMessageAsync(_message);
            }
            Console.ReadLine();
        }

        static CloudQueue GetCloudQueue()
        {
            CloudStorageAccount queue_acc = CloudStorageAccount.Parse(queue_connection_string);

            CloudQueueClient queue_client = queue_acc.CreateCloudQueueClient();

            CloudQueue _queue = queue_client.GetQueueReference(queue_name);

            return _queue;
        }
    }
}
