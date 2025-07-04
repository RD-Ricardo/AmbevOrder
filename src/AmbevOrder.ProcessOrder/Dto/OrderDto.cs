using AmbevOrder.ProcessOrder.Entities;

namespace AmbevOrder.ProcessOrder.Dto
{


    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid ExternId { get; set; }
        public string CustomerName { get; set; } = null!;
        public decimal FreightPrice { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string? ErrorMessage { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDto> Items { get; set; } = [];
    }

    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
