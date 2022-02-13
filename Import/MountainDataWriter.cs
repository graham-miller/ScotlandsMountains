namespace ScotlandsMountains.Import;

internal class MountainDataWriter
{
    private readonly Repository _repository;

    public MountainDataWriter(IOptions<CosmosConfig> config, ILogger logger)
    {
        _repository = new Repository(config, logger);
    }

    public async Task Write(IMountainData data)
    {
        await _repository.CreateDatabase();
        await _repository.Save(data.Mountains);
        await _repository.Save(data.Classifications);
        await _repository.Save(data.Sections);
        await _repository.Save(data.Counties);
        await _repository.Save(data.Maps);
    }
}