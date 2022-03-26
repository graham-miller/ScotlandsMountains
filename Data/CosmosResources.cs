namespace ScotlandsMountains.Data;

public interface ICosmosResources
{
    Task DropAndRecreateDatabase();
}

public class CosmosResources : CosmosFacade, ICosmosResources
{
    private static readonly string PartitionKey = $"/{nameof(Entity.PartitionKey).Camelize()}";

    public CosmosResources(CosmosConfig config)
        : base(config)
    { }

    public async Task DropAndRecreateDatabase()
    {
        await DropDatabase();
        var database = await CreateDatabase();
        await CreateMountainsContainer(database);
        await CreateMountainGroupsContainer(database);
    }

    private async Task DropDatabase()
    {
        var response = await GetClient().CreateDatabaseIfNotExistsAsync(Config.DatabaseId, Config.DatabaseThroughput);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            await response.Database.DeleteAsync();
        }
    }

    private async Task<Database> CreateDatabase()
    {
        var response = await GetClient().CreateDatabaseIfNotExistsAsync(Config.DatabaseId, Config.DatabaseThroughput);
        return response.Database;
    }

    private async Task CreateMountainsContainer(Database database)
    {
        var properties = new ContainerProperties(Config.MountainsContainerId, PartitionKey);
        properties.IndexingPolicy.ExcludedPaths.Add(new ExcludedPath { Path = "/*" });
        properties.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = "/name/?" });
        properties.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = "/aliases/*" });
        properties.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = "/parent/id/?" });
        properties.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = "/height/metres/?" });

        properties.GeospatialConfig.GeospatialType = GeospatialType.Geography;
        properties.IndexingPolicy.SpatialIndexes.Add(new SpatialPath { Path = "/location/*", SpatialTypes = { SpatialType.Point } });

        await database.CreateContainerIfNotExistsAsync(properties);
    }

    private async Task CreateMountainGroupsContainer(Database database)
    {
        var properties = new ContainerProperties(Config.MountainGroupsContainerId, PartitionKey);
        properties.IndexingPolicy.ExcludedPaths.Add(new ExcludedPath { Path = "/*" });
        properties.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = "/name/?" });
        properties.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = "/displayOrder/?" });

        await database.CreateContainerIfNotExistsAsync(properties);
    }
}