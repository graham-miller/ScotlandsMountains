using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScotlandsMountains.Infrastructure.Database;
using ScotlandsMountains.MigrationService;
using ScotlandsMountains.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ScotlandsMountainsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(AspireConstants.Database)));

builder.Services.AddHostedService<MigrationWorker>();

var host = builder.Build();
await host.RunAsync();