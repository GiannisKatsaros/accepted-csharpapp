namespace CSharpApp.Application.Interfaces;

public interface IExternalApiClient
{
    Task<HttpResponseMessage> Get(string url, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> Post(string url, object body, CancellationToken cancellationToken = default);
}