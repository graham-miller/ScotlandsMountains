using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScotlandsMountains.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScotlandsMountains.Infrastructure.Database.EntityConfiguration;

internal class MountainConfiguration : IEntityTypeConfiguration<Mountain>
{
    public void Configure(EntityTypeBuilder<Mountain> builder)
    {
    }
}
