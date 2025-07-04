using AmbevOrder.ProcessOrder.Dto;

namespace AmbevOrder.ProcessOrder.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
    }
}
