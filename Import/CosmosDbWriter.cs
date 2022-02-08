using System.Diagnostics;

namespace ScotlandsMountains.Import
{
    internal class CosmosDbWriter
    {
        public int Count { get; private set; }
        public double Cost { get; private set; }
        public int Errors { get; private set; }

        public async Task Write(IDobihData data)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var uri = "https://localhost:8081";
            var key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            var options = new CosmosClientOptions
            {
                AllowBulkExecution = true,
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                },
                MaxRetryAttemptsOnRateLimitedRequests = 20,
                MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(300)
            };
            var client = new CosmosClient(uri, key, options);
            var database = (await client.CreateDatabaseIfNotExistsAsync("sm")).Database;
            var container = (await database.CreateContainerIfNotExistsAsync("sm", $"/{nameof(Entity.PartitionKey).Camelize()}", 400)).Container;

            var tasks = data.Mountains
                .Select(x => container
                    .CreateItemAsync(x, new PartitionKey(x.PartitionKey))
                    .ContinueWith(HandleResponse))
                .ToList();

            await Task.WhenAll(tasks);

            stopwatch.Stop();
            var time = stopwatch.Elapsed;
        }

        private void HandleResponse(Task<ItemResponse<Mountain>> task)
        {
            Count++;

            if (task.Status == TaskStatus.RanToCompletion)
            {
                Cost += task.Result.RequestCharge;
            }
            else
            {
                Errors++;
            }
        }
    }
}
