using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;

internal interface ICountiesFactory
{
    List<County> BuildFrom(DobihRecords records);
}

internal class CountiesFactory : ICountiesFactory
{
    public List<County> BuildFrom(DobihRecords records)
    {
        return records
            .SelectMany(line => line.County?.Split('/') ?? [])
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct()
            .OrderBy(name => name)
            .Select(name => new County(name))
            .ToList();
    }
}
