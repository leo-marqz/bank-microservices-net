using Bank.Transaction.Api.Application.Database;
using Bank.Transaction.Api.Domain.Entities.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Bank.Transaction.Api.Persistence.Database
{
    public class DatabaseService : DbContext, IDatabaseService
    {
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(DbContextOptions options, ILogger<DatabaseService> logger) 
            : base(options)
        {
            _logger = logger;
        }

        public DbSet<TransactionEntity> Transaction { get; set; }

        public async Task<bool> SaveAsync()
        {
            try
            {
                _logger.LogInformation("Saving changes to the database.");
                return await SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes to the database.");
                return false;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _logger.LogInformation("Configuring the model.");
            modelBuilder.Entity<TransactionEntity>().Property(x => x.Amount).HasPrecision(18, 2);
        }
    }
}
