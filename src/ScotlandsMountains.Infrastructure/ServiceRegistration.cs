using Microsoft.Extensions.DependencyInjection;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Infrastructure.Database;
using ScotlandsMountains.Infrastructure.Storage;

namespace ScotlandsMountains.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IScotlandsMountainsDbContext, ScotlandsMountainsDbContext>();
        services.AddScoped<IFileStorageService, AzureBlobStorageService>();
        services.AddScoped<IFileUploadNotificationService, AzureServiceBusNotificationService>();

        return services;
    }
}