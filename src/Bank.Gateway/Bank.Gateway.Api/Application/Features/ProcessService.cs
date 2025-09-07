using Bank.Gateway.Api.Application.External.ServiceBusSender;
using Bank.Gateway.Api.Application.Models;
using Bank.Gateway.Api.Domain.Constants;

namespace Bank.Gateway.Api.Application.Features
{
    public class ProcessService : IProcessService
    {
        private readonly IServiceBusSenderService _serviceBusSenderService;
        public ProcessService(IServiceBusSenderService serviceBusSenderService)
        {
            _serviceBusSenderService = serviceBusSenderService;
        }

        public async Task Execute(EndpointModel model)
        {
            var modelEvent = new {
                CorrelationId = Guid.NewGuid().ToString(),
                Amount = model.Amount,
                SourceAccount = model.SourceAccount,
                DestinationAccount = model.DestinationAccount,
                CustomerId = model.CustomerId
            };
            await _serviceBusSenderService.Execute(
                    modelEvent, 
                    SendSubscriptionConstants.TRANSACTION_INITIATED
                );
        }
    }
}
