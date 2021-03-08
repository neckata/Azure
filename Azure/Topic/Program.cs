using Microsoft.Azure.ServiceBus;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Topic
{
    class Program
    {
        private static string _bus_connectionstring = "";
        private static string _topic_name = "apptopic";
        private static string _subscription_name = "SubscriptionA";
        private static ITopicClient _client;
        private static ISubscriptionClient _subscriptionClient;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        static async Task Send()
        {
            _client = new TopicClient(_bus_connectionstring, _topic_name);

            for (int i = 0; i < 10; i++)
            {
                Order obj = new Order();
                var _message = new Message(Encoding.UTF8.GetBytes(obj.ToString()));
                Console.WriteLine($"Sending Message : {obj.Id} ");
                await _client.SendAsync(_message);
            }

            Console.ReadKey();
            await _client.CloseAsync();
        }

        static async Task TopicFunction()
        {
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(_bus_connectionstring);
            _subscriptionClient = new SubscriptionClient(builder, _subscription_name);

            var _options = new MessageHandlerOptions(ExceptionReceived)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _subscriptionClient.RegisterMessageHandler(Process_Message, _options);
            Console.ReadKey();
        }


        static async Task Process_Message(Message _message, CancellationToken _token)
        {
            Console.WriteLine(Encoding.UTF8.GetString(_message.Body));
            await _subscriptionClient.CompleteAsync(_message.SystemProperties.LockToken);
        }

        static Task ExceptionReceived(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine(args.Exception);
            return Task.CompletedTask;
        }
    }
}
