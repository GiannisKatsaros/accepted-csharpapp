using System.Net.Http.Json;
using CSharpApp.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace CSharpApp.Infrastructure.HttpClients;

public class EscuelajsHttpClient : IExternalApiClient
{
    private readonly HttpClient _httpClient;

    public EscuelajsHttpClient(HttpClient httpClient, 
        IOptions<RestApiSettings> restApiSettings)
    {
        _httpClient = httpClient;
        var settings = restApiSettings.Value;
        _httpClient.BaseAddress = new Uri(settings.BaseUrl!);
    }
    
    public async Task<HttpResponseMessage> Get(string url, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        return await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
    
    public async Task<HttpResponseMessage> Post(string url, object body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = JsonContent.Create(body);
        return await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}