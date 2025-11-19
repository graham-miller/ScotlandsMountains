using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Domain.Entities;
using ScotlandsMountains.Domain.Values;
using ScotlandsMountains.Infrastructure.Database.Converters;

namespace ScotlandsMountains.Infrastructure.Database;

/*
To create a new migration, use the following command in the Package Manager Console:
    
    Add-Migration InitialCreate -Project src\ScotlandsMountains.Infrastructure -StartupProject src\ScotlandsMountains.Api -OutputDir Database\Migrations
*/

public class ScotlandsMountainsDbContext : DbContext, IScotlandsMountainsDbContext
{
    public DbSet<DobihFile> DobihFiles { get; set; }

    public DbSet<Mountain> Mountains { get; set; }

    public DbSet<Classification> Classifications { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Map> Maps { get; set; }

    public DbSet<MapPublisher> MapPublishers { get; set; }

    public DbSet<MapSeries> MapSeries { get; set; }

    public DbSet<Region> Regions { get; set; }

    public DbSet<County> Counties { get; set; }

    public ScotlandsMountainsDbContext(DbContextOptions<ScotlandsMountainsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScotlandsMountainsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}