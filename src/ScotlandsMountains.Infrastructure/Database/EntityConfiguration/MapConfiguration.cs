using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Infrastructure.Database.EntityConfiguration;

internal class MapConfiguration : EntityConfigurationBase<Map>
{
    public override void Configure(EntityTypeBuilder<Map> builder)
    {
        base.Configure(builder);
    }
}
