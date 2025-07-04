using AmbevOrder.ProcessOrder.Dto;

namespace AmbevOrder.ProcessOrder.Services.Interfaces
{
    public interface IProcessOrderService
    {
        Task ProcessOrderAsync(OrderCreateDto orderCreateDto);
    }
}

