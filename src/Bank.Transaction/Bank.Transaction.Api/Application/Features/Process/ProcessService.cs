using Bank.Transaction.Api.Application.Database;
using Bank.Transaction.Api.Domain.Constants;
using Bank.Transaction.Api.Domain.Entities.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Bank.Transaction.Api.Application.Features.Process
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
                case ReceivedSubscriptionsConstants.TRANSACTION_INITIATED:
                    TransactionInitiated(message);
                    break;
                case ReceivedSubscriptionsConstants.BALANCE_CONFIRMED:
                    BalanceConfirmed(message);
                    break;
                case ReceivedSubscriptionsConstants.BALANCE_FAILED:
                    BalanceFailed(message);
                    break;
                case ReceivedSubscriptionsConstants.TRANSFER_FAILED:
                    TransferFailed(message);
                    break;
                case ReceivedSubscriptionsConstants.TRANSFER_CONFIRMED:
                    await TransferConfirmed(message);
                    break;
            }
        }

        private void TransferFailed(string message)
        {
            throw new NotImplementedException();
        }

        private void BalanceFailed(string message)
        {
            throw new NotImplementedException();
        }

        private void BalanceConfirmed(string message)
        {
            throw new NotImplementedException();
        }

        private void TransactionInitiated(string message)
        {
            throw new NotImplementedException();
        }

        private async Task TransferConfirmed(string message)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionEntity> ProcessDatabase(TransactionEntity entity)
        {
            var entityExists = await _databaseService
                                        .Transaction
                                        .FirstOrDefaultAsync((x) => x.CorrelationId == entity.CorrelationId);
            if (entityExists is null)
            {
                entity.TransactionDate = DateTime.UtcNow;
                
                await _databaseService.Transaction.AddAsync(entity);
                await _databaseService.SaveAsync();
                
                return entity;
            }
            else
            {
                entityExists.TransactionDate = DateTime.UtcNow;
                entityExists.CurrentState = entity.CurrentState;

                _databaseService.Transaction.Update(entityExists);
                
                await _databaseService.SaveAsync();
                
                return entityExists;
            }
        }
    }
}
