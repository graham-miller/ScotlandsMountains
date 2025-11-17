using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ScotlandsMountains.Application.RequestMediator;

public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}

public class Mediator(IServiceProvider provider) : IMediator
{
    internal static void RegisterRequestHandlers(IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMediator, Mediator>();

        var types = typeof(Mediator).Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition);

        var handlerInterface = typeof(IRequestHandler<,>);

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();

            foreach (var @interface in interfaces)
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == handlerInterface)
                {
                    builder.Services.AddTransient(@interface, type);
                }
            }
        }
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var type = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = provider.GetRequiredService(type);
        var method = type.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.HandleAsync));

        return await (Task<TResponse>)method!.Invoke(handler, [request, cancellationToken])!;
    }
}
