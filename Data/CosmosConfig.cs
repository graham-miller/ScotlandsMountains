namespace ScotlandsMountains.Data;

public class CosmosConfig
{
    private CosmosClient? _client;

    public string CosmosUri { get; set; } = "https://localhost:8081";

    public string CosmosKey { get; set; } = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    
    public int DatabaseThroughput { get; set; } = 400;
    
    public string DatabaseName { get; set; } = "scotlands-mountains";
    
    public string MountainsContainerName { get; set; } = "mountains";
    
    public string MountainGroupsContainerName { get; set; } = "mountain-groups";
    
    public int BatchSize { get; set; } = 25;
    
    public int MaxRetries { get; set; } = 20;
    
    public int MaxRetryWaitSeconds { get; set; } = 300;

    public CosmosClient GetClient()
    {
        if (_client == null)
        {
            var options = new CosmosClientOptions()
            {
                AllowBulkExecution = true,
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                },
                MaxRetryAttemptsOnRateLimitedRequests = MaxRetries,
                MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(MaxRetryWaitSeconds)
            };

            _client = new CosmosClient(CosmosUri, CosmosKey, options);
        }

        return _client;
    }
}