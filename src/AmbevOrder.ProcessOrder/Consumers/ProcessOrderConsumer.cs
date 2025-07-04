using AmbevOrder.ProcessOrder.Dto;
using AmbevOrder.ProcessOrder.Services.Interfaces;
using Azure.Messaging.ServiceBus;

namespace AmbevOrder.ProcessOrder.Consumers
{
    public class ProcessOrderConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IConfiguration _configuration;

        public ProcessOrderConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var serviceBus = new ServiceBusClient(_configuration["ServiceBusConnection"], new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });

            await using var processor = serviceBus.CreateProcessor("orders-input", new ServiceBusProcessorOptions());

            processor.ProcessMessageAsync += ProcessMessage;
            processor.ProcessErrorAsync += ProcessMessageError;

            await processor.StartProcessingAsync(stoppingToken);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
        }

        private async Task ProcessMessage(ProcessMessageEventArgs eventArgs)
        {
            var scope = _serviceProvider.CreateScope();

            var processOrderService = scope.ServiceProvider.GetRequiredService<IProcessOrderService>();

            try
            {
                var messageBody = eventArgs.Message.Body.ToString();

                var createOrderDto = System.Text.Json.JsonSerializer.Deserialize<OrderCreateDto>(messageBody);

                Console.WriteLine($"Received message: {messageBody}");

                if (createOrderDto == null)
                {
                    Console.WriteLine("Received null OrderCreateDto, abandoning message.");
                    await eventArgs.AbandonMessageAsync(eventArgs.Message);
                    return;
                }

                await processOrderService.ProcessOrderAsync(createOrderDto);

                await eventArgs.CompleteMessageAsync(eventArgs.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                await eventArgs.AbandonMessageAsync(eventArgs.Message);
            }
        }

        private async Task ProcessMessageError(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"Error processing message: {eventArgs.Exception.Message}");
            Console.WriteLine($"Error source: {eventArgs.ErrorSource}");
            Console.WriteLine($"Entity path: {eventArgs.EntityPath}");
            Console.WriteLine($"Fully qualified namespace: {eventArgs.FullyQualifiedNamespace}");
            await Task.CompletedTask;
        }
    }
}
