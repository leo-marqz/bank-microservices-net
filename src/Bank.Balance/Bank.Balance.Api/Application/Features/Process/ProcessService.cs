using Bank.Balance.Api.Application.Database;
using Bank.Balance.Api.Application.External.ServiceBusSender;
using Bank.Balance.Api.Domain.Constants;
using Bank.Balance.Api.Domain.Entities.Transaction;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bank.Balance.Api.Application.Features.Process
{
    public class ProcessService : IProcessService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IServiceBusSenderService _serviceBusSenderService;

        public ProcessService(IDatabaseService databaseService, IServiceBusSenderService serviceBusSenderService)
        {
            _databaseService = databaseService;
            _serviceBusSenderService = serviceBusSenderService;
        }

        public async Task Execute(string message, string subscription)
        {
            switch (subscription)
            {
                case ReceivedSubscriptionsConstants.BALANCE_INITIATED:
                    await BalanceInitiated(message);
                    break;
                case ReceivedSubscriptionsConstants.TRANSFER_CONFIRMED_BALANCE:
                    await TransferConfirmedBalance(message);
                    break;
                case ReceivedSubscriptionsConstants.TRANSFER_FAILED_BALANCE:
                    await TransferFailedBalance(message);
                    break;

            }
        }

        private async Task TransferFailedBalance(string message)
        {
            throw new NotImplementedException();
        }

        private async Task TransferConfirmedBalance(string message)
        {
            throw new NotImplementedException();
        }

        private async Task BalanceInitiated(string message)
        {
            var entity = JsonConvert.DeserializeObject<BalanceEntity>(message);
            entity.CurrentState = CurrentStateConstants.PENDING;

            var saveEntity = await ProcessDatabase(entity);

            var eventModel = new {entity.CorrelationId, entity.CustomerId, entity.CurrentState};

            if(saveEntity.Id != 0)
            {
                //mensage para transaction
                await _serviceBusSenderService.Execute(
                    eventModel: eventModel,
                    subscription: SendSubscriptionConstants.BALANCE_CONFIRMED
                    );

            }
            else
            {
                //mensage para transaction
                await _serviceBusSenderService.Execute(
                    eventModel: eventModel,
                    subscription: SendSubscriptionConstants.BALANCE_FAILED
                    );
            }

        }

        public async Task<BalanceEntity> ProcessDatabase(BalanceEntity entity)
        {
            var entityExists = await _databaseService.Balance
                                 .FirstOrDefaultAsync((x) => x.CorrelationId == entity.CorrelationId);
            if (entityExists is null)
            {
                entity.BalanceDate = DateTime.UtcNow;

                _databaseService.Balance.Add(entity);
                await _databaseService.SaveAsync();

                return entity;
            }
            else
            {
                entityExists.BalanceDate = DateTime.UtcNow;
                entityExists.CurrentState = entity.CurrentState;

                _databaseService.Balance.Update(entityExists);

                await _databaseService.SaveAsync();

                return entityExists;
            }
        }

    }
}
