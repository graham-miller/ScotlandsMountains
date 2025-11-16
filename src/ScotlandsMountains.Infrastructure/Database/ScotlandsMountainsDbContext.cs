using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ScotlandsMountains.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ScotlandsMountains.Infrastructure.Database;

/*
To create a new migration, use the following command in the Package Manager Console:
    
    Add-Migration InitialCreate -Project src\ScotlandsMountains.Infrastructure -StartupProject src\ScotlandsMountains.Api -OutputDir Database\Migrations
*/

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