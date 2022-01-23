namespace ScotlandsMountains.Data;

public class MountainsBatch
{
    private readonly DateTime _startedAt = DateTime.UtcNow;
    private readonly Container _container;

    public int Count { get; private set;}
    public double Cost { get; private set;}
    public int Errors { get; private set;}
    public TimeSpan ExecutionTime { get; private set; }

    public MountainsBatch(Container container)
    {
        _container = container;
    }

    public async Task Insert(IEnumerable<Mountain> mountains)
    {
        await Task.WhenAll(
            mountains.Select(WriteToCosmosDb));

        SetComplete();
        LogStats();
    }

    private Task WriteToCosmosDb(Mountain mountain)
    {
        return _container
            .CreateItemAsync(mountain, new PartitionKey(mountain.PartitionKey))
            .ContinueWith(HandleResponse);
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

    private void SetComplete()
    {
        ExecutionTime = DateTime.Now.Subtract(_startedAt);
    }

    private void LogStats()
    {
        Console.WriteLine($"{nameof(MountainsBatch)} batch written to Cosmos DB, count: {Count}, errors: {Errors}, cost: {Cost:0.00} RUs, time: {ExecutionTime}.");
    }
}