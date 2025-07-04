using Azure.Messaging.ServiceBus;

namespace AmbevOrder.ProcessOrder.Consumers
{
    public class QueryOrderConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IConfiguration _configuration;

        public QueryOrderConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
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

            await using var processor = serviceBus.CreateProcessor("orders-query", new ServiceBusProcessorOptions());

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

            // Exemplo se você precisar de um serviço específico, guardar em um banco de dados, etc.

            try
            {
                var messageBody = eventArgs.Message.Body.ToString();

                Console.WriteLine($"Received message: {messageBody}");

                await eventArgs.CompleteMessageAsync(eventArgs.Message);
            }
            catch (Exception ex)
            {
                await eventArgs.AbandonMessageAsync(eventArgs.Message);
            }
        }

        private async Task ProcessMessageError(ProcessErrorEventArgs eventArgs)
        {
            await Task.CompletedTask;
        }
    }
}
