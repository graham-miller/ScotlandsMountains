using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ScotlandsMountains.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ScotlandsMountains.MigrationService;

public class MigrationWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationWorker(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Create a new scope to resolve scoped services
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ScotlandsMountainsDbContext>();

        // Apply migrations
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(
            context,
            async (dbContext, _, cancellationToken) =>
            {
                await dbContext.Database.MigrateAsync(cancellationToken);
                return true;
            },
            null,
            stoppingToken
        );

        // Optionally add seeding logic here...

        // The worker can stop itself after migration
        var lifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
        lifetime.StopApplication();
    }
}