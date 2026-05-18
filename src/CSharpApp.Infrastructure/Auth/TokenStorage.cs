using System.IdentityModel.Tokens.Jwt;

namespace CSharpApp.Infrastructure.Auth;

public class TokenStorage
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public string? AccessToken { get; private set; }
    public string? RefreshToken { get; private set; }
    private DateTime _accessTokenExpiresAt;
    private DateTime _refreshTokenExpiresAt;

    private bool IsAccessTokenExpired => DateTime.UtcNow >= _accessTokenExpiresAt;
    public bool IsRefreshTokenExpired => DateTime.UtcNow >= _refreshTokenExpiresAt;

    public async Task<string> GetOrRefresh(
        Func<Task<string>> fetchToken,
        CancellationToken cancellationToken = default)
    {
        if (AccessToken is not null && !IsAccessTokenExpired)
            return AccessToken;

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (AccessToken is not null && !IsAccessTokenExpired)
                return AccessToken;

            return await fetchToken();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void SetTokens(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        _accessTokenExpiresAt = ExtractExpiry(accessToken);
        _refreshTokenExpiresAt = ExtractExpiry(refreshToken);
    }

    private static DateTime ExtractExpiry(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return jwt.ValidTo;
    }
}