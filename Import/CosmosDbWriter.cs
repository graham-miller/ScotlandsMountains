using System.Diagnostics;

namespace ScotlandsMountains.Import;

internal class CosmosDbWriter
{
    private const string CosmosUri = "https://localhost:8081";
    private const string CosmosKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    private const int DatabaseThroughput = 400;
    private const int ContainerThroughput = 400;
    private const string DatabaseName = "sm";
    private const string ContainerName = "sm";
    private const int BatchSize = 25;
    private const int MaxRetries = 20;
    private const int MaxRetryWaitSeconds = 300;

    private Container? _container;
    private int _count;
    private int _cost;
    private int _errors;
    private List<string> _statuses = new();
    private readonly Stopwatch _stopwatch = new ();

    public async Task Write(IDobihData data)
    {
        _stopwatch.Start();

        await CreateContainer();

        await Save(data.Mountains);
        await Save(data.Classifications);
        await Save(data.Sections);
        await Save(data.Counties);
        await Save(data.Maps);
    }

    private async Task CreateContainer()
    {
        var options = new CosmosClientOptions
        {
            AllowBulkExecution = true,
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            },
            MaxRetryAttemptsOnRateLimitedRequests = MaxRetries,
            MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(MaxRetryWaitSeconds)
        };
        var client = new CosmosClient(CosmosUri, CosmosKey, options);
        var database = (await client.CreateDatabaseIfNotExistsAsync(DatabaseName, DatabaseThroughput)).Database;
        _container = (await database.CreateContainerIfNotExistsAsync(ContainerName, $"/{nameof(Entity.PartitionKey).Camelize()}", ContainerThroughput)).Container;
    }

    private async Task Save<T>(IReadOnlyCollection<T> items) where T : Entity
    {
        ResetCounters();

        var batchCounter = 0;
        var batchCount = items.Count / BatchSize + (items.Count % BatchSize == 0 ? 0 : 1);
            
        foreach (var batch in items.Batch(BatchSize))
        {

            batchCounter++;
                
            _statuses.Add($"{typeof(T).Name.Pluralize()}: saving batch {batchCounter:#,##0} of {batchCount:#,##0}");
            UpdateConsole();

            var tasks = batch
                .Select(x => _container!
                    .CreateItemAsync(x, new PartitionKey(x.PartitionKey))
                    .ContinueWith(HandleResponse))
                .ToList();

            await Task.WhenAll(tasks);

            _statuses = _statuses.Take(_statuses.Count - 1).ToList();

        }

        _statuses.Add($"{typeof(T).Name.Pluralize()} saved, count: {_count:#,##0}, errors: {_errors:#,##0}, cost: {_cost:#,##0} RUs, time: {_stopwatch.Elapsed:g}");
        UpdateConsole();
    }

    private void ResetCounters()
    {
        _count = 0;
        _cost = 0;
        _errors = 0;
    }

    private void UpdateConsole()
    {
        Console.Clear();
        _statuses.ForEach(Console.WriteLine);
    }

    private void HandleResponse<T>(Task<ItemResponse<T>> task) where T : Entity
    {
        Interlocked.Increment(ref _count);

        if (task.Status == TaskStatus.RanToCompletion)
        {
            Interlocked.Add(ref _cost, (int) task.Result.RequestCharge);
        }
        else
        {
            Interlocked.Increment(ref _errors);
        }
    }
}