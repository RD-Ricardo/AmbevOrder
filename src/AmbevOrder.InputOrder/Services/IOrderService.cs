using AmbevOrder.InputOrder.Dtos;

namespace AmbevOrder.InputOrder.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderCreateDto orderCreateDto);
    }
}
