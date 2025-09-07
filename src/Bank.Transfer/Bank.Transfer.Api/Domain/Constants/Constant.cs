﻿namespace Bank.Transfer.Api.Domain.Constants
{
    ////////////////API Transfer//////////////
    public static class CurrentStateConstants
    {
        public const string PENDING = "Pending";
    }
    public class ReceivedSubscriptionsConstants
    {
        public const string TRANSFER_INITIATED = "transfer-initiated";
    }
    public class SendSubscriptionConstants
    {
        public const string TRANSFER_CONFIRMED = "transfer-confirmed";
        public const string TRANSFER_FAILED = "transfer-failed";
    }

}
