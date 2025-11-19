using Microsoft.Extensions.DependencyInjection;

namespace ScotlandsMountains.Application.Ports;

public interface IRequest<TResponse> { }

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}

public class Mediator : IMediator
{
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    internal static void Register(IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

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
                    services.AddTransient(@interface, type);
                }
            }
        }
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var type = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = _provider.GetRequiredService(type);
        var method = type.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.HandleAsync));

        return await (Task<TResponse>)method!.Invoke(handler, [request, cancellationToken])!;
    }
}
