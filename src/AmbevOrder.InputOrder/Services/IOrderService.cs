using AmbevOrder.InputOrder.Dtos;

namespace AmbevOrder.InputOrder.Services
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(OrderCreateDto orderCreateDto);
    }
}
