using AmbevOrder.ProcessOrder.Dto;
using AmbevOrder.ProcessOrder.Repositories;
using AmbevOrder.ProcessOrder.Services.Interfaces;

namespace AmbevOrder.ProcessOrder.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                ExternId = order.ExternId,
                CustomerName = order.CustomerName,
                FreightPrice = order.FreightPrice,
                ProcessedAt = order.ProcessedAt,
                PaidAt = order.PaidAt,
                CreatedAt = order.CreatedAt,
                Status = order.Status.ToString(),
                ErrorMessage = order.ErrorMessage,
                TotalPrice = order.TotalPrice,
                Items = order.Items.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            }).ToList();
        }
    }
}
