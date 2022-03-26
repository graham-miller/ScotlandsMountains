using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(ScotlandsMountains.Api.Startup))]

namespace ScotlandsMountains.Api;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddOptions<CosmosConfig>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(nameof(CosmosConfig)).Bind(settings);
            });

        builder.Services.AddSingleton<ICosmosContainers, CosmosContainers>();
        builder.Services.AddTransient<IMountainsRepository, MountainsRepository>();
        builder.Services.AddTransient<CosmosConfig>(services => services.GetService<IOptions<CosmosConfig>>()?.Value);
    }
}