using Bank.Balance.Api.Domain.Entities.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Bank.Balance.Api.Application.Database
{
    public interface IDatabaseService
    {
        DbSet<BalanceEntity> Balance { get; set; }
        Task<bool> SaveAsync();
    }
}
