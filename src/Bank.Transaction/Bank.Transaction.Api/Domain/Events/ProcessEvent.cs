﻿
using MediatR;

namespace Bank.Transaction.Api.Domain.Events
{
    public class ProcessEvent : INotification
    {
        public string Message { get; set; }
        public string Subscription { get; set; }

        public ProcessEvent(string message, string subscription)
        {
            Message = message;
            Subscription = subscription;
        }
    }
}
