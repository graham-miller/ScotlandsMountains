using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Infrastructure.Database.EntityConfiguration;

internal class CountryConfiguration : EntityConfigurationBase<Country>
{
    public override void Configure(EntityTypeBuilder<Country> builder)
    {
        base.Configure(builder);
    }
}
