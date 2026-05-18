using System.Text.Json.Serialization;

namespace CSharpApp.Infrastructure.Auth;

public record AuthResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = null!;
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; } = null!;
}