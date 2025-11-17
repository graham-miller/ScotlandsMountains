using Microsoft.Extensions.Hosting;

namespace ScotlandsMountains.Infrastructure;

public static class ServiceRegistration
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        return builder;
    }
}