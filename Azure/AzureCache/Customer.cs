namespace AzureCache
{
    class Customer
    {
        public int customerID { get; set; }
        public string customerName { get; set; }

        public Customer(int p_id, string p_name)
        {
            this.customerID = p_id;
            this.customerName = p_name;
        }
    }
}
