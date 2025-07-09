namespace Medio.Core.Abstractions;

public interface IMediator
{
    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);

    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}