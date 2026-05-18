using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace CSharpApp.Infrastructure.Auth;

public class AuthService(HttpClient httpClient, TokenStorage tokenStorage, IOptions<RestApiSettings> restApiSettings)
{
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    
    public async Task<string> GetAccessToken(CancellationToken cancellationToken = default)
    {
        return tokenStorage switch
        {
            { AccessToken: not null, IsAccessTokenExpired: false } => tokenStorage.AccessToken,
            { RefreshToken: not null, IsRefreshTokenExpired: false } => await RefreshAccessToken(cancellationToken),
            _ => await Login(cancellationToken)
        };
    }

    private async Task<string> Login(CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_restApiSettings.BaseUrl}auth/login");
        request.Content = JsonContent.Create(new
        {
            email = _restApiSettings.Username,
            password = _restApiSettings.Password
        });

        var response = await httpClient.SendAsync(request, cancellationToken);
        return await HandleAuthResponse(response, cancellationToken);
    }

    private async Task<string> RefreshAccessToken(CancellationToken cancellationToken = default)
    {
         var request = new HttpRequestMessage(HttpMethod.Post, $"{_restApiSettings.BaseUrl}auth/refresh-token");
         
         request.Content = JsonContent.Create(new
         {
             refreshToken = tokenStorage.RefreshToken
         });
         
         var response = await httpClient.SendAsync(request, cancellationToken);
         return await HandleAuthResponse(response, cancellationToken);
    }

    private async Task<string> HandleAuthResponse(HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var authResponse = JsonSerializer.Deserialize<AuthResponse>(content);
         
        tokenStorage.SetTokens(authResponse.AccessToken, authResponse.RefreshToken);
        
        return authResponse.AccessToken;
    }
}