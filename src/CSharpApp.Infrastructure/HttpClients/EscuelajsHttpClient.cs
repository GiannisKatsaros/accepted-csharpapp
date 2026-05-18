using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CSharpApp.Application.Interfaces;
using CSharpApp.Infrastructure.Auth;
using Microsoft.Extensions.Options;

namespace CSharpApp.Infrastructure.HttpClients;

public class EscuelajsHttpClient : IExternalApiClient
{
    private readonly HttpClient _httpClient;
    private readonly AuthService  _authService;

    public EscuelajsHttpClient(HttpClient httpClient, 
        IOptions<RestApiSettings> restApiSettings, AuthService authService)
    {
        _httpClient = httpClient;
        var settings = restApiSettings.Value;
        _authService = authService;
        _httpClient.BaseAddress = new Uri(settings.BaseUrl!);
    }
    
    public async Task<HttpResponseMessage> Get(string url, CancellationToken cancellationToken = default)
    {
        var request = await BuildRequest(HttpMethod.Get, url, cancellationToken);
        return await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
    
    public async Task<HttpResponseMessage> Post(string url, object body, CancellationToken cancellationToken = default)
    {
        var request = await BuildRequest(HttpMethod.Post, url, cancellationToken);
        request.Content = JsonContent.Create(body);
        return await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
    
    private async Task<HttpRequestMessage> BuildRequest(HttpMethod method, string url, CancellationToken cancellationToken = default)
    {
        var token = await _authService.GetAccessToken(cancellationToken);
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }
}