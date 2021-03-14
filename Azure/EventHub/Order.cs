namespace EventHub
{
    public class Order
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public int Quantity { get; set; }

        public Order(int p_id,string p_category,int p_quantity)
        {
            Id = p_id;
            Category = p_category;
            Quantity = p_quantity;
        }

        public override string ToString()
        {
            return $"Id : {Id}, Category : {Category} , Quantity {Quantity}";
        }
    }
}
