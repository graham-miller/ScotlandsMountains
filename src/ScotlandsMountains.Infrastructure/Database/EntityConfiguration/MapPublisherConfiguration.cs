using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Infrastructure.Database.EntityConfiguration;

internal class MapPublisherConfiguration : EntityConfigurationBase<MapPublisher>
{
    public override void Configure(EntityTypeBuilder<MapPublisher> builder)
    {
        base.Configure(builder);
    }
}
