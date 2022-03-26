namespace ScotlandsMountains.Data;

public class CosmosConfig
{
    public string Uri { get; set; } // = "https://localhost:8081";

    public string Key { get; set; } // = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

    public int DatabaseThroughput { get; set; } // = 1000;

    public string DatabaseId { get; set; } // = "scotlands-mountains";

    public string MountainsContainerId { get; set; } // = "mountains";

    public string MountainGroupsContainerId { get; set; } // = "mountain-groups";

    public int BatchSize { get; set; } // = 25;

    public int MaxRetries { get; set; } // = 20;

    public int MaxRetryWaitSeconds { get; set; } // = 300;
}