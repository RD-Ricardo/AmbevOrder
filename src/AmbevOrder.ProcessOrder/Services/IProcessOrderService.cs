using AmbevOrder.ProcessOrder.Dto;

namespace AmbevOrder.ProcessOrder.Services
{
    public interface IProcessOrderService
    {
        Task ProcessOrderAsync(OrderCreateDto orderCreateDto);
    }
}

