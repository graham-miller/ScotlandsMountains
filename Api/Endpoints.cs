namespace ScotlandsMountains.Api;

public class Endpoints
{
    private readonly MountainGroupsRepository _repository;

    public Endpoints()
    {
        var config = Options.Create(new CosmosConfig());
        _repository = new MountainGroupsRepository(config);
    }

    // http://localhost:7071/api/classifications
    [FunctionName(nameof(GetClassifications))]
    public async Task<IActionResult> GetClassifications(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "classifications")] HttpRequest request,
        ILogger logger)
    {
        var result = await _repository.GetClassifications();
        return new OkObjectResult(result);
    }

    // http://localhost:7071/api/classifications/{id}
    [FunctionName(nameof(GetClassification))]
    public async Task<IActionResult> GetClassification(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "classifications/{id}")] HttpRequest request,
        string id,
        ILogger logger)
    {
        var result = await _repository.GetClassification(id);

        if (result == null) return new NotFoundResult();

        return new OkObjectResult(result);
    }

    // http://localhost:7071/api/mountains/{id}
    [FunctionName(nameof(GetMountain))]
    public async Task<IActionResult> GetMountain(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "mountains/{id}")] HttpRequest request,
        string id,
        ILogger logger)
    {
        var result = await _repository.GetMountain(id);

        if (result == null) return new NotFoundResult();

        return new OkObjectResult(result);
    }

    // http://localhost:7071/api/search?term={term}
    [FunctionName(nameof(Search))]
    public async Task<IActionResult> Search(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", "POST", Route = "search")] HttpRequest request,
        ILogger logger)
    {
        var term = request.Query.GetString("term");
        if (string.IsNullOrWhiteSpace(term)) return new BadRequestResult();

        var pageSize = request.Query.GetInt("pageSize") ?? 10;

        string continuationToken = null;
        if (request.Method == HttpMethods.Post)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            continuationToken = JsonConvert.DeserializeObject<dynamic>(json).continuationToken;
        }

        var result = await _repository.Search(term, pageSize, continuationToken);
        return new OkObjectResult(result);
    }
}