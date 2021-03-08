using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace AzureCache
{
    class Program
    {
        static void Main(string[] args)
        {
            IDatabase l_cache = redisconn.Value.GetDatabase();
            //Set a value in the redis database
            //l_cache.StringSet("AvgOrders", "120");

            // If you want to set an expiration for the keys
            //l_cache.KeyExpire("AvgOrders", new TimeSpan(0, 0, 30));


            // To delete a key
            //l_cache.KeyDelete("AvgOrders");

            //Console.WriteLine("The Average Daily Orders are " + l_cache.StringGet("AvgOrders"));

            // You can also work with class objects
            // Add the object to the cache

            Customer obj = new Customer(10, "userA");
            l_cache.StringSet("obj1", JsonConvert.SerializeObject(obj));

            // Get the object from the cache
            Customer newobj = JsonConvert.DeserializeObject<Customer>(l_cache.StringGet("obj1"));
            Console.WriteLine("The Customer ID is " + newobj.customerID);
            Console.WriteLine("The Customer Name is " + newobj.customerName);

            redisconn.Value.Dispose();
            Console.ReadKey();
        }

        // The connection to Azure Cache for Redis is managed via the ConenctionMultiplexer class
        private static Lazy<ConnectionMultiplexer> redisconn = new Lazy<ConnectionMultiplexer>(() =>
        {
            string str_conn = "appcache4000.redis.cache.windows.net:6380,password=riV+9bT5b97H6V4na0ZKD86DMnJ2EmoIncDPko12oKE=,ssl=True,abortConnect=False";
            return ConnectionMultiplexer.Connect(str_conn);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return redisconn.Value;
            }
        }
    }
}
