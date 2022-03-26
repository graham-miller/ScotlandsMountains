namespace ScotlandsMountains.Data;

public interface ICosmosResources
{
    Task DropAndRecreateDatabase();
}

public class CosmosResources : CosmosFacade, ICosmosResources
{
    private static readonly string PartitionKey = $"/{nameof(Entity.PartitionKey).Camelize()}";

    public CosmosResources(IOptions<CosmosConfig> config)
        : base(config)
    { }

    public async Task DropAndRecreateDatabase()
    {
        await DropDatabase();
        await CreateDatabase();
    }

    private async Task DropDatabase()
    {
        var response = await GetClient().CreateDatabaseIfNotExistsAsync(Config.DatabaseId, Config.DatabaseThroughput);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            await response.Database.DeleteAsync();
        }
    }

    private async Task CreateDatabase()
    {
        var response = await GetClient().CreateDatabaseIfNotExistsAsync(Config.DatabaseId, Config.DatabaseThroughput);
        var database = response.Database;

        await database.CreateContainerIfNotExistsAsync(Config.MountainsContainerId, PartitionKey);
        await database.CreateContainerIfNotExistsAsync(Config.MountainGroupsContainerId, PartitionKey);
    }
}