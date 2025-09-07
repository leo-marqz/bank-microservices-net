using Azure.Messaging.ServiceBus;
using Bank.Gateway.Api.Application.External.ServiceBusSender;
using System.Text.Json;

namespace Bank.Gateway.Api.External.ServiceBusSender
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
            _logger.LogInformation("Sending message to topic {TopicName} with subscription {Subscription}", _topicName, subscription);
            await using var sender = _serviceBusClient.CreateSender(_topicName);
            var messageBody = JsonSerializer.Serialize(eventModel);

            ServiceBusMessage message = new ServiceBusMessage(messageBody)
            {
                ContentType = "application/json",
                Subject = subscription //nos ayudara con los filtros en cada suscripcion
            };

            await sender.SendMessageAsync(message);
            _logger.LogInformation("Message sent to topic {TopicName} with subscription {Subscription}", _topicName, subscription);
        }
    }
}
