using Bank.Transaction.Api.Application.Database;
using Bank.Transaction.Api.Application.External.ServiceBusSender;
using Bank.Transaction.Api.Domain.Constants;
using Bank.Transaction.Api.Domain.Entities.Transaction;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;

namespace Bank.Transaction.Api.Application.Features.Process
{
    public class ProcessService : IProcessService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IServiceBusSenderService _serviceBusSenderService;
        private readonly ILogger<ProcessService> _logger;

        public ProcessService(IDatabaseService databaseService, 
            IServiceBusSenderService serviceBusSenderService,
            ILogger<ProcessService> logger)
        {
            _databaseService = databaseService;
            _serviceBusSenderService = serviceBusSenderService;
            _logger = logger;
        }

        public async Task Execute(string message, string subscription)
        {
            _logger.LogInformation("(Transaction) Processing message for subscription: {Subscription}", subscription);
            switch (subscription)
            {
                case ReceivedSubscriptionsConstants.TRANSACTION_INITIATED:
                    _logger.LogInformation("(Transaction) Handling TRANSACTION_INITIATED");
                    await TransactionInitiatedAsync(message);
                    break;
                case ReceivedSubscriptionsConstants.BALANCE_CONFIRMED:
                    _logger.LogInformation("(Transaction) Handling BALANCE_CONFIRMED");
                    BalanceConfirmed(message);
                    break;
                case ReceivedSubscriptionsConstants.BALANCE_FAILED:
                    _logger.LogInformation("(Transaction) Handling BALANCE_FAILED");
                    BalanceFailed(message);
                    break;
                case ReceivedSubscriptionsConstants.TRANSFER_FAILED:
                    _logger.LogInformation("(Transaction) Handling TRANSFER_FAILED");
                    TransferFailed(message);
                    break;
                case ReceivedSubscriptionsConstants.TRANSFER_CONFIRMED:
                    _logger.LogInformation("(Transaction) Handling TRANSFER_CONFIRMED");
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

        private async Task TransactionInitiatedAsync(string message)
        {
            var entity = JsonConvert.DeserializeObject<TransactionEntity>(message);

            if(entity is null)
            {
                _logger.LogError("(Transaction)(TransactionInitiatedAsync) Failed to deserialize message: {Message}", message);
                return;
            }

            entity.CurrentState = CurrentStateConstants.PENDING;
            
            var saveEntity = await ProcessDatabase(entity);

            var eventModel = new { saveEntity.CorrelationId, saveEntity.CustomerId };

            if(saveEntity.Id != 0)
            {
                await _serviceBusSenderService.Execute(
                    eventModel, 
                    SendSubscriptionConstants.BALANCE_INITIATED);
            }
            else
            {
                await _serviceBusSenderService.Execute(
                    eventModel, 
                    SendSubscriptionConstants.TRANSACTION_FAILED);
            }
        }

        private async Task TransferConfirmed(string message)
        {
            throw new NotImplementedException();
        }

        private async Task<TransactionEntity> ProcessDatabase(TransactionEntity entity)
        {
            var entityExists = await _databaseService.Transaction
                                 .FirstOrDefaultAsync((x) => x.CorrelationId == entity.CorrelationId);
            if (entityExists is null)
            {
                entity.TransactionDate = DateTime.UtcNow;

                _databaseService.Transaction.Add(entity);
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
