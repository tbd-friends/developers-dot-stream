using System.Text.Json.Serialization;

namespace Developers.Stream.Infrastructure.YouTube;

public class YouTubeAuthenticationResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = null!;
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("id_token")] public string IdToken { get; set; } = null!;
    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; } = null!;
    public string[] Scope { get; set; } = null!;
    [JsonPropertyName("token_type")] public string TokenType { get; set; } = null!;

    public bool IsValid => !string.IsNullOrWhiteSpace(AccessToken);

    public static YouTubeAuthenticationResponse Invalid => new();
}