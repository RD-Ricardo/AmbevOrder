using System.Text.Json;
using AmbevOrder.InputOrder.Dtos;
using Azure.Messaging.ServiceBus;

namespace AmbevOrder.InputOrder.Services
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration _configuration;
        public OrderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task CreateOrderAsync(OrderCreateDto orderCreateDto)
        {
            if (orderCreateDto == null)
            {
                throw new ArgumentNullException(nameof(orderCreateDto), "OrderCreateDto cannot be null");
            }

            orderCreateDto.Id = Guid.NewGuid();

            if (orderCreateDto.Items == null || !orderCreateDto.Items.Any())
            {
                throw new ArgumentException("Order must contain at least one item", nameof(orderCreateDto.Items));
            }

            orderCreateDto.Items.ForEach(item =>
            {
                if (item.Id == Guid.Empty)
                {
                    item.Id = Guid.NewGuid();
                }
            });

            var serviceBusClient = new ServiceBusClient(_configuration["ServiceBusConnection"]);
            
            var serviceBusSender = serviceBusClient.CreateSender("orders-input");

            var orderMessage = new ServiceBusMessage(JsonSerializer.Serialize(orderCreateDto))
            {
                ContentType = "application/json"
            };

            await serviceBusSender.SendMessageAsync(orderMessage);
        }
    }
}
