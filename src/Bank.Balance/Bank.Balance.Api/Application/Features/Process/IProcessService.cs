﻿namespace Bank.Balance.Api.Application.Features.Process
{
    public interface IProcessService
    {
        Task Execute(string message, string subscription);
    }
}
