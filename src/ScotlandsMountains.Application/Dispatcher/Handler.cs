namespace ScotlandsMountains.Application.Dispatcher;

public abstract class Handler<TRequest, TResponse>
{
    public abstract TResponse Handle(TRequest request);
}
