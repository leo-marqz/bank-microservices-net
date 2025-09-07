using Bank.Transaction.Api.Domain.Constants;

namespace Bank.Transaction.Api.Application.Features.Process
{
    public class ProcessService : IProcessService
    {
        public ProcessService() { }

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
    }
}
