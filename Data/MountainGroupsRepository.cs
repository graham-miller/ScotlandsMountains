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

    public async IAsyncEnumerable<dynamic> GetClassifications()
    {
        var iterator = _client
            .GetDatabase(_config.DatabaseName)
            .GetContainer(_config.MountainGroupsContainerName)
            .GetItemLinqQueryable<Classification>()
            .Where(c => c.PartitionKey == nameof(Classification).Camelize())
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
                c.MountainsCount
            })
            .ToFeedIterator();

        var response = await iterator.ReadNextAsync();
     
        foreach (var item in response)
            yield return item;
    }

    public async Task<dynamic?> GetClassification(Guid id)
    {
        var iterator = _client
            .GetDatabase(_config.DatabaseName)
            .GetContainer(_config.MountainGroupsContainerName)
            .GetItemLinqQueryable<Classification>()
            .Where(c => c.PartitionKey == nameof(Classification).Camelize() && c.Id == id)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.SingularName,
                c.Description,
                c.Mountains,
                c.MountainsCount
            })
            .ToFeedIterator();

        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            return response.SingleOrDefault();
        }

        return null;
    }

    public async Task<dynamic?> GetMountain(Guid id)
    {
        var iterator = _client
            .GetDatabase(_config.DatabaseName)
            .GetContainer(_config.MountainsContainerName)
            .GetItemLinqQueryable<Mountain>()
            .Where(m => m.PartitionKey == nameof(Mountain).Camelize() && m.Id == id)
            .Select(m => new
            {
                m.Id,
                m.Name,
                m.Aliases,
                m.Location,
                m.GridRef,
                m.Height,
                m.Prominence,
                m.Features,
                m.Observations,
                m.Parent,
                m.Section,
                m.Counties,
                m.Classifications,
                m.Maps
            })
            .ToFeedIterator();

        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            return response.SingleOrDefault();
        }

        return null;
    }

    public async Task<dynamic> Search(string term, int pageSize, string? base64EncodedContinuationToken)
    {
        var requestOptions = new QueryRequestOptions { MaxItemCount = pageSize };

        var continuationToken = string.IsNullOrWhiteSpace(base64EncodedContinuationToken)
            ? null
            : Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(base64EncodedContinuationToken));

        var iterator = _client
            .GetDatabase(_config.DatabaseName)
            .GetContainer(_config.MountainsContainerName)
            .GetItemLinqQueryable<Mountain>(false, continuationToken, requestOptions)
            .Where(m =>
                m.PartitionKey == nameof(Mountain).Camelize() &&
                (m.Name.Contains(term, StringComparison.InvariantCultureIgnoreCase) ||
                m.Aliases.Any(a => a.Contains(term, StringComparison.InvariantCultureIgnoreCase))))
            .OrderByDescending(m => m.Height.Metres)
            .Select(m => new
            {
                m.Id,
                m.Name,
                m.Aliases,
                m.Location,
                m.Height
            })
            .ToFeedIterator();

        var results = new List<dynamic>();

        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);

            continuationToken = response.Count > 0
                ? WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(response.ContinuationToken))
                : null;
        }

        return new { results, term, continuationToken };
    }
}
