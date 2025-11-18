using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Infrastructure.Storage;

namespace ScotlandsMountains.Infrastructure;

public static class ServiceRegistration
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFileStorageService, AzureBlobStorageService>();
        builder.Services.AddScoped<IFileUploadNotificationService, AzureServiceBusNotificationService>();

        return builder;
    }
}