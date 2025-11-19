using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Infrastructure.Database.EntityConfiguration;

internal class CountyConfiguration : EntityConfigurationBase<County>
{
    public override void Configure(EntityTypeBuilder<County> builder)
    {
        base.Configure(builder);
    }
}
