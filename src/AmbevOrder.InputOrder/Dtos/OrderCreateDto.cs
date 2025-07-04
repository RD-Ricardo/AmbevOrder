namespace AmbevOrder.InputOrder.Dtos
{
    public class OrderCreateDto
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public decimal FreightPrice { get; set; }
        public List<Item> Items { get; set; } = null!;
    }

    public class Item 
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
