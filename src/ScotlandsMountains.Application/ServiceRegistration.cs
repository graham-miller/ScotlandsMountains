using Microsoft.Extensions.DependencyInjection;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;

namespace ScotlandsMountains.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Mediator.Register(services);

        services.AddScoped<IDobihFileReader, DobihFileReader>();
        services.AddScoped<IDobihRecordsParserFactory, DobihRecordsParserFactory>();

        return services;
    }
}