using Aspire.Hosting.Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ScotlandsMountains.Infrastructure.Database;

namespace ScotlandsMountains.AppHost.Extensions;

internal static class ResourceBuilderExtensions
{
    public static IResourceBuilder<ProjectResource> WithSwaggerUrls(this IResourceBuilder<ProjectResource> builder)
    {
        return builder.WithUrls(context =>
        {
            context.Urls.ForEach(url =>
            {
                if (string.IsNullOrEmpty(url.DisplayText))
                {
                    url.Url += "/swagger";
    
                    if (url?.Endpoint?.Scheme == "http")
                        url.DisplayLocation = UrlDisplayLocation.DetailsOnly;
                    }
            });
        });
    }

    internal static IResourceBuilder<AzureStorageResource> RunAsEmulatorWithDefaultPorts(this IResourceBuilder<AzureStorageResource> builder)
    {
        builder.RunAsEmulator(azurite =>
        {
            azurite.WithEndpoint("blob", e => e.Port = 10000);
            azurite.WithEndpoint("queue", e => e.Port = 10001);
            azurite.WithEndpoint("table", e => e.Port = 10002);
        });

        return builder;
    }

    public static IResourceBuilder<SqlServerServerResource> WithScotlandsMountainsDataBindMount(this IResourceBuilder<SqlServerServerResource> builder)
    {
        var path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".scotlandsmountains",
            "sql-data");

        Directory.CreateDirectory(path);

        return builder.WithDataBindMount(source: path);
    }

    public static IResourceBuilder<SqlServerDatabaseResource> WithMigrationsCommands(this IResourceBuilder<SqlServerDatabaseResource> builder)
    {
        return builder
            .WithCommand(
                "migrate", "Migrate", context => OnRunMigrationCommand(builder, context, deleteFirst: false),
                new CommandOptions
                {
                    Description = "Run migrations.",
                    UpdateState = OnUpdateResourceState,
                    IconName = "ArrowCircleUpSparkle",
                    IconVariant = IconVariant.Regular,
                    ConfirmationMessage = "Are you sure you want to run migrations on the database?"

                })
            .WithCommand(
                "reset", "Reset", context => OnRunMigrationCommand(builder, context, deleteFirst: true),
                new CommandOptions
                {
                    Description = "Drop database then run migrations.",
                    UpdateState = OnUpdateResourceState,
                    IconName = "BroomSparkle",
                    IconVariant = IconVariant.Regular,
                    ConfirmationMessage = "Are you sure you want to reset the database? This will delete all data.",
                });
    }

    private static async Task<ExecuteCommandResult> OnRunMigrationCommand(
        IResourceBuilder<SqlServerDatabaseResource> builder,
        ExecuteCommandContext context,
        bool deleteFirst)
    {
        try
        {
            var connectionString = await builder.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);

            var options = new DbContextOptionsBuilder<ScotlandsMountainsDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var dbContext = new ScotlandsMountainsDbContext(options))
            {
                if (deleteFirst)
                {
                    await dbContext.Database.EnsureDeletedAsync(context.CancellationToken);
                }

                await dbContext.Database.MigrateAsync(context.CancellationToken);
            }

            return CommandResults.Success();
        }
        catch (Exception ex)
        {
            return CommandResults.Failure(ex.Message);
        }
    }

    static ResourceCommandState OnUpdateResourceState(UpdateCommandStateContext context)
    {
        return context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy
            ? ResourceCommandState.Enabled
            : ResourceCommandState.Disabled;
    }
}