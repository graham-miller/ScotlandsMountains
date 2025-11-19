using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Infrastructure.Database.EntityConfiguration;

internal class RegionConfiguration : EntityConfigurationBase<Region>
{
    public override void Configure(EntityTypeBuilder<Region> builder)
    {
        base.Configure(builder);
    }
}
