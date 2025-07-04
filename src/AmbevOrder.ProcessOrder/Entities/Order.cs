namespace AmbevOrder.ProcessOrder.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid ExternId { get; private set; }
        public string CustomerName { get; private set; } = null!;
        public decimal FreightPrice { get; private set; }
        public List<OrderItem> Items { get; private set; } = null!;
        public DateTime? ProcessedAt { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public OrderStatus Status { get; private set; }
        public string? ErrorMessage { get; private set; }
        public decimal TotalPrice { get; private set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public Order() { }
        public Order(Guid externId, string customerName, decimal freightPrice)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName;
            FreightPrice = freightPrice;
            ExternId = externId;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.Pending;
            ProcessedAt = null;
            PaidAt = null;
            ErrorMessage = null;
            TotalPrice = 0;
        }

        public void AddItems(List<OrderItem> items)
        {
            if (items == null || !items.Any())
                throw new ArgumentException("Order must contain at least one item.", nameof(items));
            if (Items == null)
                Items = new List<OrderItem>();
            foreach (var item in items)
            {
                if (item.Id == Guid.Empty)
                    item.Id = Guid.NewGuid();
            }
            Items.AddRange(items);
        }

        public void Processed()
        {
            if (ProcessedAt.HasValue)
                throw new InvalidOperationException("Order has already been processed.");

            ProcessedAt = DateTime.UtcNow;
            Status = OrderStatus.Processed;
        }

        public void Cancelled()
        {
            if (Status == OrderStatus.Processed)
                throw new InvalidOperationException("Cannot cancel a processed order.");

            ProcessedAt = DateTime.UtcNow;
            Status = OrderStatus.Cancelled;
        }

        public void UpdateFreightPrice(decimal freightPrice)
        {
            if (freightPrice < 0)
                throw new ArgumentException("Freight price cannot be negative.", nameof(freightPrice));
            FreightPrice = freightPrice;
        }

        public void UpdateErrorMessage(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Error message cannot be null or empty.", nameof(errorMessage));
            ErrorMessage = errorMessage;
        }

        public void Paid()
        {
            if (PaidAt.HasValue)
                throw new InvalidOperationException("Order has already been paid.");
            PaidAt = DateTime.UtcNow;
        }

        public void CalculateTotal()
        {
            if (Items == null || !Items.Any())
                throw new InvalidOperationException("Cannot calculate total for an order with no items.");

            TotalPrice = Items.Sum(item => item.Price * item.Quantity) + FreightPrice;
            
            if (TotalPrice < 0)
            {
                throw new InvalidOperationException("Total value cannot be negative.");
            }
        }
    }

    public enum OrderStatus
    {
        Pending,
        Processed,
        Cancelled
    }
}
