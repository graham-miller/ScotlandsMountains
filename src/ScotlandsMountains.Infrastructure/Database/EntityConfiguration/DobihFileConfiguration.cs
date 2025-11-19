using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Infrastructure.Database.EntityConfiguration;

internal class DobihFileConfiguration : EntityConfigurationBase<DobihFile>
{
    public override void Configure(EntityTypeBuilder<DobihFile> builder)
    {
        base.Configure(builder);
    }
}
