namespace ScotlandsMountains.Data;

public class Repository
{
    private readonly ILogger _logger;
    private readonly CosmosConfig _config;
    private readonly CosmosClient _client;

    public Repository(IOptions<CosmosConfig> config, ILogger logger)
    {
        _logger = logger;
        _config = config.Value;
        _client = _config.GetClient();
    }

    public async Task<Database> CreateDatabase()
    {
        var response = await _client.CreateDatabaseIfNotExistsAsync(_config.DatabaseName, _config.DatabaseThroughput);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            await response.Database.DeleteAsync();
            return (await _client.CreateDatabaseIfNotExistsAsync(_config.DatabaseName, _config.DatabaseThroughput)).Database;
        }

        return response.Database;
    }

    public async Task Save<T>(IReadOnlyCollection<T> items) where T : Entity
    {
        var containerName = typeof(T) == typeof(Mountain)
            ? _config.MountainsContainerName
            : _config.MountainGroupsContainerName;
        var database = _client.GetDatabase(_config.DatabaseName);
        var container = (await database!.CreateContainerIfNotExistsAsync(containerName, $"/{nameof(Entity.PartitionKey).Camelize()}")).Container;

        var batchCounter = 0;
        var batchCount = items.Count / _config.BatchSize + (items.Count % _config.BatchSize == 0 ? 0 : 1);
        var count = 0;
        var cost = 0;
        var errors = 0;

        _logger.LogInformation($"Saving {typeof(T).Name.Pluralize()}, batches: {batchCount:#,##0}");

        foreach (var batch in items.Batch(_config.BatchSize))
        {
            batchCounter++;
            _logger.LogDebug($"Saving {typeof(T).Name.Pluralize()} batch {batchCounter:#,##0} of {batchCount:#,##0}");

            var tasks = batch
                .Select(x => container
                    .CreateItemAsync(x, new PartitionKey(x.PartitionKey))
                    .ContinueWith(task =>
                    {
                        Interlocked.Increment(ref count);

                        if (task.Status == TaskStatus.RanToCompletion)
                            Interlocked.Add(ref cost, (int)task.Result.RequestCharge);
                        else
                            Interlocked.Increment(ref errors);
                    }))
                .ToList();

            await Task.WhenAll(tasks);
        }

        _logger.LogInformation($"Completed saving {typeof(T).Name.Pluralize()}, count: {count:#,##0}, errors: {errors:#,##0}, cost: {cost:#,##0} RUs");
    }
}