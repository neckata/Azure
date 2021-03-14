using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs.Consumer;
using System.Threading;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs.Processor;

namespace EventHub
{
    class Program
    {
        private static string _bus_connection = "Endpoint=sb://appname.servicebus.windows.net/;SharedAccessKeyName=demo;SharedAccessKey=Dz60v7IXFFmFGWVMGSfmONyouCHa0S3/IvQINDZ3rAw=;EntityPath=apphub";
        private static string _hubname = "apphub";
        private static string _storage_account = "DefaultEndpointsProtocol=https;AccountName=demostore3000;AccountKey=uTR9XQL25G/uxoimMo8xusantfPwhrdNXcssNA0g7Od5pM4Uzxl9C+i0U1iIRCM3JucXq9/F34NceJ85yHNdiw==;EndpointSuffix=core.windows.net";
        private static string _container = "processor";

        static string[] cities = new string[]
        {
            "Burgas","Varna","Sofia","Plovdiv"
        };

        static void Main(string[] args)
        {
            SendData().Wait();
            //GetEvents().Wait();
            //SetProccessorSubscribe().Wait();
        }

        private static async Task SendData()
        {
            EventHubProducerClient client = new EventHubProducerClient(_bus_connection, _hubname);
            string _partition = (await client.GetPartitionIdsAsync()).First();

            var _options = new CreateBatchOptions
            {
                PartitionId = _partition
            };

            EventDataBatch batch_obj = await client.CreateBatchAsync(_options);
            Random _rnd = new Random();
            for (int i = 1; i <= 10; i++)
            {
                Order obj = new Order(i, cities[_rnd.Next(0, 4)], _rnd.Next(1, 100));
                batch_obj.TryAdd(new EventData(Encoding.UTF8.GetBytes(obj.ToString())));

            }
            await client.SendAsync(batch_obj);

            Console.WriteLine("Sent all Orders");
        }

        private static async Task GetEvents()
        {
            EventHubConsumerClient client = new EventHubConsumerClient("$Default", _bus_connection, _hubname);

            string _partition = (await client.GetPartitionIdsAsync()).First();

            var cancellation = new CancellationToken();

            EventPosition _position = EventPosition.FromSequenceNumber(5);
            Console.WriteLine("Getting events from a certain position from a particular partition");

            await foreach (PartitionEvent _recent_event in client.ReadEventsFromPartitionAsync(_partition, _position, cancellation))
            {
                EventData event_data = _recent_event.Data;

                Console.WriteLine(Encoding.UTF8.GetString(event_data.Body.ToArray()));
                Console.WriteLine($"Sequence Number : {event_data.SequenceNumber}");
            }
        }

        private static async Task SetProccessorSubscribe()
        {
            BlobContainerClient _blob_client = new BlobContainerClient(_storage_account, _container);

            EventProcessorClient _event_client = new EventProcessorClient(_blob_client, "$Default", _bus_connection, _hubname);

            _event_client.ProcessEventAsync += Process_Message;
            _event_client.ProcessErrorAsync += Error_Handler;

            await _event_client.StartProcessingAsync();

            await Task.Delay(TimeSpan.FromSeconds(100));

            await _event_client.StopProcessingAsync();

            Console.WriteLine("Completed");
        }

        private static async Task Process_Message(ProcessEventArgs eventArgs)
        {
            Console.WriteLine("Getting the events");
            Console.WriteLine(Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));

            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        private static Task Error_Handler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine("An Error has occurred");
            Console.WriteLine(eventArgs.Exception.Message);

            return Task.CompletedTask;
        }
    }
}
