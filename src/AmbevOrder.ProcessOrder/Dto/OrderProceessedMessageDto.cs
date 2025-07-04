namespace AmbevOrder.ProcessOrder.Dto
{
    public class OrderProceessedMessageDto
    {
        public Guid OrderId { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string OrderStatus { get; set; }
        public decimal Total { get; set; }
        public OrderProceessedMessageDto(Guid orderId, DateTime processedAt, string orderStatus, decimal total)
        {
            OrderId = orderId;
            ProcessedAt = processedAt;
            OrderStatus = orderStatus;
            Total = total;
        }
    }
}
