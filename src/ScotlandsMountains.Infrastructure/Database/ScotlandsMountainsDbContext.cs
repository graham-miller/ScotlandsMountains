using Microsoft.EntityFrameworkCore;
using ScotlandsMountains.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScotlandsMountains.Infrastructure.Database;

public class ScotlandsMountainsDbContext : DbContext
{
    public DbSet<Mountain> Mountains { get; set; }

    public ScotlandsMountainsDbContext(DbContextOptions<ScotlandsMountainsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScotlandsMountainsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}