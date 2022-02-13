using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScotlandsMountains.Data;

public class MountainGroupsRepository
{
    private readonly CosmosConfig _config;
    private readonly CosmosClient _client;

    public MountainGroupsRepository(IOptions<CosmosConfig> config)
    {
        _config = config.Value;
        _client = _config.GetClient();
    }

    public async Task<JArray> GetClassifications()
    {
        const string query = @"
                SELECT
                    c.id,
                    c.name,
                    c.description
                FROM c
                WHERE c.partitionKey = 'classification'
                ORDER BY c.displayOrder ASC";

        return await QueryMountainGroupsContainer(query);
    }

    public async Task<JToken?> GetClassification(string id)
    {
        var query = $@"
                SELECT
                    c.id,
                    c.name,
                    c.description,
                    c.mountains
                FROM c
                WHERE c.partitionKey = 'classification'
                AND c.id = '{id}'
                ORDER BY c.displayOrder ASC";

        var result = await QueryMountainGroupsContainer(query);

        return result.Any() ? result.First() : null;
    }

    public async Task<JToken?> GetMountain(string id)
    {
        var query = $@"
                SELECT
                    c.name,
                    c.aliases,
                    c.location,
                    c.gridRef,
                    c.height,
                    c.prominence,
                    c.features,
                    c.observations,
                    c.parent,
                    c.region,
                    c.county,
                    c.classifications,
                    c.maps,
                    c.id,
                    c.partitionKey
                FROM c
                WHERE c.partitionKey = '{id}'
                AND c.id = '{id}'";

        var result = await QueryMountainsContainer(query);

        return result.Any() ? result.First() : null;
    }

    public async Task<JObject> Search(string term, int pageSize, string? continuationToken)
    {
        var query = $@"
                SELECT
                    c.name,
                    c.location,
                    c.height,
                    c.id,
                    c.partitionKey
                FROM c
                WHERE CONTAINS(c.name, {"\""}{term.Replace("\"", "\\\"")}{"\""}, true)
                ORDER BY c.height.metres DESC";

        return await SearchMountainAspectsContainer(query, term, pageSize, continuationToken);
    }

    private async Task<JArray> QueryMountainsContainer(string query)
    {
        return await QueryCosmosDb(query, _config.MountainsContainerName);
    }

    private async Task<JArray> QueryMountainGroupsContainer(string query)
    {
        return await QueryCosmosDb(query, _config.MountainGroupsContainerName);
    }

    private async Task<JArray> QueryCosmosDb(string query, string containerId)
    {
        var container = _client.GetDatabase(_config.DatabaseName).GetContainer(containerId);
        var iterator = container.GetItemQueryStreamIterator(query);

        using (var memoryStream = new MemoryStream())
        {
            while (iterator.HasMoreResults)
            {
                using (var result = await iterator.ReadNextAsync())
                {
                    await result.Content.CopyToAsync(memoryStream);
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(memoryStream))
            {
                var json = await streamReader.ReadToEndAsync();
                var x = JsonConvert.DeserializeObject<JObject>(json) as IEnumerable<KeyValuePair<string, JToken>>;
                var y = (JArray)x.First(j => j.Key == "Documents").Value;

                return y;
            }
        }
    }

    private async Task<JObject> SearchMountainAspectsContainer(string query, string term, int pageSize, string? continuationToken)
    {
        var container = _client.GetDatabase(_config.DatabaseName).GetContainer(_config.MountainsContainerName);
        var options = new QueryRequestOptions { MaxItemCount = pageSize };
        var iterator = container.GetItemQueryStreamIterator(query, continuationToken, options);

        using (var memoryStream = new MemoryStream())
        {
            var result = await iterator.ReadNextAsync();
            var nextContinuationToken = result.ContinuationToken;
            await result.Content.CopyToAsync(memoryStream);

            memoryStream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(memoryStream))
            {
                var json = await streamReader.ReadToEndAsync();
                var results = JsonConvert.DeserializeObject<JObject>(json).GetValue("Documents");

                return new JObject(
                    new JProperty("term", term),
                    new JProperty("continuationToken", nextContinuationToken),
                    new JProperty("results", results)
                );
            }
        }
    }
}
