using ScotlandsMountains.Application.UseCases.DobihFiles.Models;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Factories;

internal class CountiesFactory
{
    internal static List<County> BuildFrom(DobihRecordsByNumber records)
    {
        return records.All
            .SelectMany(line => line.County?.Split('/') ?? [])
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct()
            .OrderBy(name => name)
            .Select(name => new County(name))
            .ToList();
    }
}
