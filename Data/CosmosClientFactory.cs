namespace ScotlandsMountains.Data;

public interface ICosmosClientFactory
{
    CosmosClient Get();
}

public class CosmosClientFactory : ICosmosClientFactory
{
    private readonly IOptions<CosmosConfig> _config;

    public CosmosClientFactory(IOptions<CosmosConfig> config)
    {
        _config = config;
    }

    public CosmosClient Get()
    {
        return new CosmosClient(_config.Value.ConnectionString);
    }
}