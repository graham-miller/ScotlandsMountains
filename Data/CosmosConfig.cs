namespace ScotlandsMountains.Data;

public class CosmosConfig
{
    public string ConnectionString { get; set; } = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    public string DatabaseId { get; set; } = "scotlands-mountains";
    public string ContainerId { get; set; } = "mountains";
    public int Throughput { get; set; } = 400;
    public int InsertBatchSize { get; set; } = 50;
}