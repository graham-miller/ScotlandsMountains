using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScotlandsMountains.Domain.Entities;
using ScotlandsMountains.Infrastructure.Database.Converters;

namespace ScotlandsMountains.Infrastructure.Database.EntityConfiguration;

internal class MapSeriesConfiguration : EntityConfigurationBase<MapSeries>
{
    public override void Configure(EntityTypeBuilder<MapSeries> builder)
    {
        base.Configure(builder);

        builder
            .Property(e => e.Scale)
            .HasConversion<MapScaleConverter>();
    }
}
