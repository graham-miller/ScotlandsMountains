namespace ScotlandsMountains.Data;

public abstract class CosmosFacade
{
    protected readonly CosmosConfig Config;
    private CosmosClient? _client;

    protected CosmosFacade(CosmosConfig config)
    {
        Config = config;
    }

    protected CosmosClient GetClient()
    {
        if (_client != null) return _client;

        var options = new CosmosClientOptions
        {
            AllowBulkExecution = true,
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            },
            MaxRetryAttemptsOnRateLimitedRequests = Config.MaxRetries,
            MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(Config.MaxRetryWaitSeconds)
        };

        _client = new CosmosClient(Config.Uri, Config.Key, options);

        return _client;
    }
}