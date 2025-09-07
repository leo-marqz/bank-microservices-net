using Azure.Messaging.ServiceBus;
using Bank.Transaction.Api.Application.External.ServiceBusSender;
using System.Text.Json;

namespace Bank.Transaction.Api.External.ServiceBusSender
{
    public class ServiceBusSenderService : IServiceBusSenderService
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _topicName;
        private readonly ILogger<ServiceBusSenderService> _logger;

        public ServiceBusSenderService(IConfiguration configuration, ILogger<ServiceBusSenderService> logger)
        {
            _serviceBusClient = new ServiceBusClient(configuration["SERVICE_BUS_CONNECTION_STRING"]);
            _topicName = configuration["SERVICE_BUS_TOPIC"]!;
            _logger = logger;
        }

        public async Task Execute(object eventModel, string subscription)
        {
            _logger.LogInformation("(Transaction) Sending message to Service Bus Topic: {TopicName}, Subscription: {Subscription}", _topicName, subscription);
            await using var sender = _serviceBusClient.CreateSender(_topicName);
            
            var messageBody = JsonSerializer.Serialize(eventModel);

            _logger.LogInformation("(Transaction) Message Body: {MessageBody}", messageBody);
            ServiceBusMessage message = new ServiceBusMessage(messageBody)
            {
                ContentType = "application/json",
                Subject = subscription //nos ayudara con los filtros en cada suscripcion
            };

            _logger.LogInformation("(Transaction) Sending message...");
            await sender.SendMessageAsync(message);
        }
    }
}
