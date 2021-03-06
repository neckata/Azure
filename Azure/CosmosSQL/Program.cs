using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosSQL
{
    class Program
    {
        static string database = "appdb";
        static string containername = "customer";
        static string endpoint = "https://appaccount6000.documents.azure.com:443/";
        static string accountkeys = "";

        static async Task Main(string[] args)
        {
            CreateNewItem().Wait();
            ReadItem().Wait();
            ReplaceItem().Wait();
            DeleteItem().Wait();
            Console.ReadLine();
        }

        private static async Task CreateNewItem()
        {
            using (CosmosClient cosmos_client = new CosmosClient(endpoint, accountkeys))
            {
                Database db_conn = cosmos_client.GetDatabase(database);

                Container container_conn = db_conn.GetContainer(containername);

                Customer obj = new Customer(1, "John", "Miami");
                obj.id = Guid.NewGuid().ToString();

                List<Orders> list_orders = new List<Orders>()
                {
                    new Orders(100,10), new Orders(101,20)
                };
                obj.order = list_orders;

                ItemResponse<Customer> response = await container_conn.CreateItemAsync(obj);
                Console.WriteLine("Request charge is {0}", response.RequestCharge);
                Console.WriteLine("Customer added");
            }
        }

        private static async Task ReadItem()
        {
            using (CosmosClient cosmos_client = new CosmosClient(endpoint, accountkeys))
            {
                Database db_conn = cosmos_client.GetDatabase(database);

                Container container_conn = db_conn.GetContainer(containername);

                string cosmos_sql = "select c.customerid,c.customername,c.city from c";
                QueryDefinition query = new QueryDefinition(cosmos_sql);

                FeedIterator<Customer> iterator_obj = container_conn.GetItemQueryIterator<Customer>(cosmos_sql);

                while (iterator_obj.HasMoreResults)
                {
                    FeedResponse<Customer> customer_obj = await iterator_obj.ReadNextAsync();
                    foreach (Customer obj in customer_obj)
                    {
                        Console.WriteLine("Customer id is {0}", obj.customerid);
                        Console.WriteLine("Customer name is {0}", obj.customername);
                        Console.WriteLine("Customer city is {0}", obj.city);

                        foreach (Orders ord in obj.order)
                        {
                            Console.WriteLine($"Order Id is {ord.orderid}");
                            Console.WriteLine($"Order Quantity is {ord.quantity}");
                        }
                    }
                }
            }
        }

        private static async Task ReplaceItem()
        {
            using (CosmosClient cosmos_client = new CosmosClient(endpoint, accountkeys))
            {
                Database db_conn = cosmos_client.GetDatabase(database);

                Container container_conn = db_conn.GetContainer(containername);

                PartitionKey pk = new PartitionKey("Miami");
                string id = "0e31ed86-9824-4fe6-a6de-a7853348f344";

                ItemResponse<Customer> response = await container_conn.ReadItemAsync<Customer>(id, pk);
                Customer customer_obj = response.Resource;

                customer_obj.customername = "James";

                response = await container_conn.ReplaceItemAsync<Customer>(customer_obj, id, pk);
                Console.WriteLine("Item updated");
            }
        }

        private static async Task DeleteItem()
        {
            using (CosmosClient cosmos_client = new CosmosClient(endpoint, accountkeys))
            {
                Database db_conn = cosmos_client.GetDatabase(database);

                Container container_conn = db_conn.GetContainer(containername);

                PartitionKey pk = new PartitionKey("Miami");
                string id = "0e31ed86-9824-4fe6-a6de-a7853348f344";

                ItemResponse<Customer> response = await container_conn.DeleteItemAsync<Customer>(id, pk);

                Console.WriteLine("Item deleted");
            }

        }
    }
}
