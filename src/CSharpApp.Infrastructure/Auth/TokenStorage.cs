using System.IdentityModel.Tokens.Jwt;

namespace CSharpApp.Infrastructure.Auth;

public class TokenStorage
{
    public string? AccessToken { get; private set; }
    public string? RefreshToken { get; private set; }
    private DateTime _accessTokenExpiresAt;
    private DateTime _refreshTokenExpiresAt;

    public bool IsAccessTokenExpired => DateTime.UtcNow >= _accessTokenExpiresAt;
    public bool IsRefreshTokenExpired => DateTime.UtcNow >= _refreshTokenExpiresAt;
    
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