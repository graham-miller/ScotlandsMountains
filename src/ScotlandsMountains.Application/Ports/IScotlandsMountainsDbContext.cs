using Microsoft.EntityFrameworkCore;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.Ports;

public interface IScotlandsMountainsDbContext
{
    DbSet<DobihFile> DobihFiles { get; }

    DbSet<Mountain> Mountains { get; }

    DbSet<Classification> Classifications { get; }

    DbSet<Country> Countries { get; }

    DbSet<Map> Maps { get; }

    DbSet<MapPublisher> MapPublishers { get; }

    DbSet<MapSeries> MapSeries { get; }

    DbSet<Region> Regions { get; }

    DbSet<County> Counties { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
