
using Azure.Messaging.ServiceBus;
using Bank.Transaction.Api.Domain.Constants;
using Bank.Transaction.Api.Domain.Events;
using MediatR;
using System.Threading.Tasks;

namespace Bank.Transaction.Api.External.ServiceBusReceive
{
    public class ServiceBusReceiveService : BackgroundService
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly List<ServiceBusProcessor> _processors;
        private readonly IMediator _mediator;
        private readonly ILogger<ServiceBusReceiveService> _logger;

        public ServiceBusReceiveService(IConfiguration configuration, IMediator mediator, ILogger<ServiceBusReceiveService> logger)
        {
            _mediator = mediator;
            _logger = logger;
            _serviceBusClient = new ServiceBusClient(configuration["SERVICE_BUS_CONNECTION_STRING"]);

            var subscriptions = new[]
            {
                ReceivedSubscriptionsConstants.TRANSACTION_INITIATED,
                ReceivedSubscriptionsConstants.BALANCE_CONFIRMED,
                ReceivedSubscriptionsConstants.TRANSFER_CONFIRMED,
                ReceivedSubscriptionsConstants.TRANSFER_FAILED
            };

            _processors = subscriptions.Select((subscription) =>
            {
                var processor = _serviceBusClient.CreateProcessor(configuration["SERVICE_BUS_TOPIC"], subscription);
                processor.ProcessMessageAsync += async (args) => await Process(args, subscription);
                processor.ProcessErrorAsync += ProcessError;

                return processor;
            }).ToList();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("(Transaction) Service Bus Receive Service is starting.");
            await Task.WhenAll(_processors.Select(p => p.StartProcessingAsync()));
            await Task.Run(() => stoppingToken.WaitHandle.WaitOne(), stoppingToken);
        }
        private async Task Process(ProcessMessageEventArgs args, string subscription)
        {
            _logger.LogInformation($"(Transaction) Message received from subscription: {subscription}");
            string body = args.Message.Body.ToString();
            await _mediator.Publish(new ProcessEvent(body, subscription));
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ProcessError(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, $"(Transaction) Error processing message: {args.Exception.Message}");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("(Transaction) Service Bus Receive Service is stopping.");
            await Task.WhenAll(_processors.Select(p => p.StopProcessingAsync()));
            await base.StopAsync(cancellationToken);
        }

        public override async void Dispose()
        {
            _logger.LogInformation("(Transaction) Service Bus Receive Service is disposing.");
            await Task.WhenAll(_processors.Select(p => p.DisposeAsync().AsTask() )); ;
            await _serviceBusClient.DisposeAsync();
            base.Dispose();
        }

    }
}
