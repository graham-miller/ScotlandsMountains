namespace ScotlandsMountains.Api;

public class Endpoints
{
    private readonly IMountainsRepository _repository;

    public Endpoints(IMountainsRepository repository)
    {
        _repository = repository;
    }

    // http://localhost:7071/api/classifications
    [FunctionName(nameof(GetClassifications))]
    public IActionResult GetClassifications(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "classifications")] HttpRequest request,
        ILogger logger)
    {
        return new OkObjectResult(_repository.GetClassifications());
    }

    // http://localhost:7071/api/classifications/{id}
    [FunctionName(nameof(GetClassification))]
    public async Task<IActionResult> GetClassification(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "classifications/{id}")] HttpRequest request,
        Guid id,
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
        Guid id,
        ILogger logger)
    {
        var result = await _repository.GetMountain(id);

        if (result == null) return new NotFoundResult();

        return new OkObjectResult(result);
    }

    // http://localhost:7071/api/search?term={term}&continuationToken={continuationToken}
    [FunctionName(nameof(Search))]
    public async Task<IActionResult> Search(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "search")] HttpRequest request,
        ILogger logger)
    {
        var term = request.Query.GetString("term");
        if (string.IsNullOrWhiteSpace(term)) return new BadRequestResult();

        var pageSize = request.Query.GetInt("pageSize") ?? 10;

        var continuationToken = request.Query.GetString("continuationToken");

        var result = await _repository.Search(term, pageSize, continuationToken);
        return new OkObjectResult(result);
    }
}