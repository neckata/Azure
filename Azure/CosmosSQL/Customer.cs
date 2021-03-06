using Newtonsoft.Json;
using System.Collections.Generic;

namespace CosmosSQL
{
    class Customer
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "customerid")]
        public int customerid { get; set; }

        [JsonProperty(PropertyName = "customername")]
        public string customername { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string city { get; set; }

        [JsonProperty(PropertyName = "orders")]
        public List<Orders> order;

        public Customer(int p_id, string p_name, string p_city)
        {
            customerid = p_id;
            customername = p_name;
            city = p_city;
        }
    }

    class Orders
    {
        public int orderid { get; set; }
        public int quantity { get; set; }

        public Orders(int p_id, int p_quantity)
        {
            orderid = p_id;
            quantity = p_quantity;
        }
    }
}
