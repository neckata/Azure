using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queue
{
    class Program
    {
        private static string _bus_connectionstring = "";
        private static string _queue_name = "appqueue";
        private static QueueClient _client;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        static async Task Send()
        {
            IQueueClient _client;
            _client = new QueueClient(_bus_connectionstring, _queue_name);
            
            for (int i = 1; i <= 10; i++)
            {
                Order obj = new Order();
                var _message = new Message(Encoding.UTF8.GetBytes(obj.ToString()));
                await _client.SendAsync(_message);
                Console.WriteLine($"Sending Message : {obj.Id} ");
            }
        }

        static async Task QueueFunction()
        {
            _client = new QueueClient(_bus_connectionstring, _queue_name);

            var _options = new MessageHandlerOptions(ExceptionReceived)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _client.RegisterMessageHandler(Process_Message, _options);
            Console.ReadKey();
        }

        static async Task Process_Message(Message _message, CancellationToken _token)
        {
            Console.WriteLine(Encoding.UTF8.GetString(_message.Body));


            await _client.CompleteAsync(_message.SystemProperties.LockToken);
        }

        static Task ExceptionReceived(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine(args.Exception);
            return Task.CompletedTask;
        }
    }
}
