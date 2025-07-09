using Medio.Core.Abstractions;

namespace Medio.Core.Implementations;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());
        dynamic? handler = serviceProvider.GetService(handlerType);
        if (handler == null)
            throw new InvalidOperationException($"Handler for {request.GetType().Name} not registered.");
        await handler.HandleAsync((dynamic)request, cancellationToken);
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic? handler = serviceProvider.GetService(handlerType);
        if (handler == null)
            throw new InvalidOperationException($"Handler for {request.GetType().Name} not registered.");
        return await handler.HandleAsync((dynamic)request, cancellationToken);
    }
}