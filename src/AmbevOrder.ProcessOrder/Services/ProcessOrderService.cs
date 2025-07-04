using System.Text.Json;
using AmbevOrder.ProcessOrder.Dto;
using AmbevOrder.ProcessOrder.Entities;
using AmbevOrder.ProcessOrder.Repositories;
using AmbevOrder.ProcessOrder.Services.Interfaces;
using Azure.Messaging.ServiceBus;

namespace AmbevOrder.ProcessOrder.Services
{
    public class ProcessOrderService : IProcessOrderService
    {
        private readonly IOrderRepository _orderRepository;

        private readonly ITransaction _trasaction;

        private readonly IConfiguration _configuration;

        private readonly ILogger<ProcessOrderService> _logger;
        public ProcessOrderService(IOrderRepository orderRepository, ITransaction trasaction, IConfiguration configuration, ILogger<ProcessOrderService> logger)
        {
            _orderRepository = orderRepository;
            _trasaction = trasaction;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task ProcessOrderAsync(OrderCreateDto orderCreateDto)
        {
            _trasaction.BeginTransactionAsync();

            try
            {
                var orderProcessed = await _orderRepository.GetByExternIdAsync(orderCreateDto.Id);

                if (orderProcessed is not null)
                {
                    _logger.LogInformation($"Order with ID {orderCreateDto.Id} has already been processed.");
                    return;
                }

                var order = new Order(orderCreateDto.Id, orderCreateDto.CustomerName, orderCreateDto.FreightPrice);

                order.AddItems([.. orderCreateDto.Items.Select(o => new OrderItem
                {
                    ProductName = o.ProductName,
                    Price = o.Price,
                    Quantity = o.Quantity
                })]);

                await _orderRepository.AddAsync(order);

                orderCreateDto.FreightPrice += (orderCreateDto.FreightPrice * 10) / 100;

                order.UpdateFreightPrice(orderCreateDto.FreightPrice);

                order.CalculateTotal();

                var paymentResult = SimulatePaymentAuthorized();

                if (!paymentResult.Sucecss)
                {
                    order.UpdateErrorMessage(paymentResult.ErrorMessage!);
                    
                    order.Cancelled();

                    await _orderRepository.UpdateAsync(order);

                    await SendOrderProcessed(new OrderProceessedMessageDto(order.Id, order.ProcessedAt!.Value, order.Status.ToString(), order.TotalPrice));

                    _trasaction.CommitTransactionAsync();
                    
                    return;
                }

                order.Paid();
                order.Processed();

                await _orderRepository.UpdateAsync(order);

                await SendOrderProcessed(new OrderProceessedMessageDto(order.Id, order.ProcessedAt!.Value, order.Status.ToString(), order.TotalPrice));

                _trasaction.CommitTransactionAsync();
            }
            catch (Exception)
            {
                _trasaction.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task SendOrderProcessed(OrderProceessedMessageDto message)
        {
            var serviceBusClient = new ServiceBusClient(_configuration["ServiceBusConnection"]);

            var serviceBusSender = serviceBusClient.CreateSender("orders-query");

            var orderMessage = new ServiceBusMessage(JsonSerializer.Serialize(message))
            {
                ContentType = "application/json"
            };

            await serviceBusSender.SendMessageAsync(orderMessage);
        }

        private ResultApi SimulatePaymentAuthorized()
        {
            var random = new Random();
            var succeeded = random.Next(0, 2) == 1;

            return new ResultApi
            {
                Sucecss = succeeded,
                ErrorMessage = succeeded ? null : "Pagamento não autorizado"
            };
        }
    }

    public class ResultApi
    {
        public bool Sucecss { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

