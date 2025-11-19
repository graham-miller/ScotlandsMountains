using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Infrastructure.Database.EntityConfiguration;

internal class ClassificationConfiguration : EntityConfigurationBase<Classification>
{
    public override void Configure(EntityTypeBuilder<Classification> builder)
    {
        base.Configure(builder);
    }
}
