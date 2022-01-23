namespace ScotlandsMountains.Data;

public interface IMountainRepository
{
    Task Save(IEnumerable<Mountain> mountains);
}

public class MountainRepository : IMountainRepository
{
    private readonly ICosmosClientFactory _cosmosClientFactory;
    private readonly string _databaseId;
    private readonly string _containerId;
    private readonly int _throughput;
    private readonly int _batchSize;


    public MountainRepository(
        ICosmosClientFactory cosmosClientFactory,
        IOptions<CosmosConfig> config
    )
    {
        _cosmosClientFactory = cosmosClientFactory;

        _databaseId = config.Value.DatabaseId;
        _containerId = config.Value.MountainContainerId;
        _throughput = config.Value.PurchaseContainerThroughput;
        _batchSize = config.Value.InsertBatchSize;
    }

    public async Task Save(IEnumerable<Mountain> purchases)
    {
        var batches = purchases.Batch(_batchSize);
        var container = await GetContainer();

        foreach (var batch in batches)
        {
            await InsertBatch(batch, container);
        }
    }

    protected virtual async Task InsertBatch(IEnumerable<Mountain> mountains, Container container)
    {
        var batch = new MountainsBatch(container);
        await batch.Insert(mountains);
    }

    private async Task<Container> GetContainer()
    {
        var client = _cosmosClientFactory.Get();
        await client.CreateDatabaseIfNotExistsAsync(_databaseId);
        var database = client.GetDatabase(_databaseId);
        var containerProperties = new ContainerProperties(_containerId, "/partitionKey");

        //containerProperties.IndexingPolicy.ExcludedPaths.Add(new ExcludedPath { Path = "/*" });
        //containerProperties.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = $"/{nameof(Mountain.IngestedAtUtc).Camelize()}/?" });

        await database.CreateContainerIfNotExistsAsync(containerProperties, _throughput);
        var container = database.GetContainer(_containerId);
        return container;
    }
}