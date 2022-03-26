namespace ScotlandsMountains.Data;

public class ImportRepository
{
    private const int BatchSize = 100;

    private readonly ILogger _logger;
    private readonly ICosmosResources _resources;
    private readonly ICosmosContainers _containers;

    public ImportRepository(ICosmosResources resources, ICosmosContainers containers,  ILogger logger)
    {
        _logger = logger;
        _resources = resources;
        _containers = containers;
    }

    public async Task CreateDatabase()
    {
        await _resources.DropAndRecreateDatabase();
    }

    public async Task Save<T>(IReadOnlyCollection<T> items) where T : Entity
    {
        var container = typeof(T) == typeof(Mountain)
            ? _containers.GetMountainsContainer()
            : _containers.GetMountainGroupsContainer();

        var batchCounter = 0;
        var batchCount = items.Count / BatchSize + (items.Count % BatchSize == 0 ? 0 : 1);
        var count = 0;
        var cost = 0;
        var errors = 0;

        _logger.LogInformation($"Saving {typeof(T).Name.Pluralize()}, batches: {batchCount:#,##0}");

        foreach (var batch in items.Batch(BatchSize))
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