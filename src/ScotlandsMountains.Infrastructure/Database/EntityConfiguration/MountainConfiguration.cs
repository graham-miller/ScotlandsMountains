using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScotlandsMountains.Domain.Entities;
using ScotlandsMountains.Infrastructure.Database.Converters;

namespace ScotlandsMountains.Infrastructure.Database.EntityConfiguration;

internal class MountainConfiguration : EntityConfigurationBase<Mountain>
{
    public override void Configure(EntityTypeBuilder<Mountain> builder)
    {
        base.Configure(builder);

        builder
            .Property(e => e.Height)
            .HasConversion<HeightConverter>();

        builder
            .Property(e => e.GridRef)
            .HasConversion<GridRefConverter>();

        builder.Property(e => e.Coordinates)
            .HasConversion<CoordinatesConverter>();

        builder
            .Property(e => e.Drop)
            .HasConversion<HeightConverter>();

        builder
            .Property(e => e.ColGridRef)
            .HasConversion<GridRefConverter>();

        builder
            .Property(e => e.ColHeight)
            .HasConversion<HeightConverter>();


        builder
        .HasMany(e => e.Countries)
            .WithMany()
            .UsingEntity(
                "MountainCountries",
                l => l.HasOne(typeof(Country)).WithMany().HasForeignKey("CountryId"),
                r => r.HasOne(typeof(Mountain)).WithMany().HasForeignKey("MountainId"),
                j => j.HasKey("MountainId", "CountryId"));

        builder
            .HasMany(e => e.Counties)
            .WithMany()
            .UsingEntity(
                "MountainCounties",
                l => l.HasOne(typeof(County)).WithMany().HasForeignKey("CountyId"),
                r => r.HasOne(typeof(Mountain)).WithMany().HasForeignKey("MountainId"),
                j => j.HasKey("MountainId", "CountyId"));

        builder
            .HasMany(e => e.Maps)
            .WithMany(e => e.Mountains)
            .UsingEntity(
                "MountainMaps",
                l => l.HasOne(typeof(Map)).WithMany().HasForeignKey("MapId").HasPrincipalKey(nameof(Map.Id)),
                r => r.HasOne(typeof(Mountain)).WithMany().HasForeignKey("MountainId").HasPrincipalKey(nameof(Mountain.Id)),
                j => j.HasKey("MountainId", "MapId"));

        builder
            .HasMany(e => e.Classifications)
            .WithMany(e => e.Mountains)
            .UsingEntity(
                "MountainClassifications",
                l => l.HasOne(typeof(Classification)).WithMany().HasForeignKey("ClassificationId").HasPrincipalKey(nameof(Classification.Id)),
                r => r.HasOne(typeof(Mountain)).WithMany().HasForeignKey("MountainId").HasPrincipalKey(nameof(Mountain.Id)),
                j => j.HasKey("MountainId", "ClassificationId"));

    }
}
