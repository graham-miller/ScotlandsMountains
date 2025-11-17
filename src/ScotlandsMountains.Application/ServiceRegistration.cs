using Microsoft.Extensions.Hosting;
using ScotlandsMountains.Application.RequestMediator;

namespace ScotlandsMountains.Application;

public static class ServiceRegistration
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        Mediator.RegisterRequestHandlers(builder);

        return builder;
    }
}