using Bank.Notification.Api.Application.Database;
using Bank.Notification.Api.Domain.Entities.Transfer;
using Microsoft.Azure.Cosmos;

namespace Bank.Notification.Api.Persistence.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
        {
            string connectionString = configuration["NOTIFICATION_COSMOSDB_CONENECTION_STRING"]!;
            string databaseName = configuration["NOTIFICATION_COSMOSDB_DATABASE_NAME"]!;
            string containerName = configuration["NOTIFICATION_COSMOSDB_CONTAINER_NAME"]!;

            _cosmosClient = new CosmosClient(connectionString);
            _container = _cosmosClient.GetContainer(databaseName, containerName);
            this._logger = logger;
        }

        public async Task<List<NotificationEntity>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all notification entities from Cosmos DB.");
            var query = _container.GetItemQueryIterator<NotificationEntity>("SELECT * FROM c");
            var list = new List<NotificationEntity>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                list.AddRange(response);
            }
            _logger.LogInformation("Fetched {Count} notification entities from Cosmos DB.", list.Count);
            return list;
        }

        public async Task<bool> AddAsync(NotificationEntity entity)
        {
            try
            {
                entity.Id = Guid.NewGuid().ToString();
                entity.NotificationDate = DateTime.UtcNow;
                
                var response = await _container.CreateItemAsync(entity, new PartitionKey(entity.CorrelationId));
                
                if(response.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    _logger.LogError("Failed to add item to Cosmos DB. Status code: {StatusCode}", response.StatusCode);
                    return false;
                }

                return true;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Cosmos DB error while adding item");
                Console.WriteLine($"Cosmos DB error: {ex.StatusCode} - {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error while adding item");
                Console.WriteLine($"General error: {ex.Message}");
                return false;
            }
        }
    }
}
