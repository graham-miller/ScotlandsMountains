using Microsoft.Extensions.DependencyInjection;
using ScotlandsMountains.Application.Ports;

namespace ScotlandsMountains.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Mediator.Register(services);

        return services;
    }
}