using Bank.Balance.Api.Application.Database;
using Bank.Balance.Api.Domain.Constants;
using Bank.Balance.Api.Domain.Entities.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Bank.Balance.Api.Application.Features.Process
{
    public class ProcessService : IProcessService
    {
        private readonly IDatabaseService _databaseService;

        public ProcessService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
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
            throw new NotImplementedException();
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
