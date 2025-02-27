using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace Developers.Stream.Infrastructure.Twitch;

public class TwitchClient(
    HttpClient client,
    IOptions<TwitchConfiguration> configuration
    ) : ITwitchClient
{
    private readonly TwitchConfiguration _configuration = configuration.Value;

    public async Task<TwitchAuthenticationResponse> FetchAuthenticationFromCode(string code)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/oauth2/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _configuration.ClientId,
                ["client_secret"] = _configuration.Secret,
                ["code"] = code,
                ["grant_type"] = "authorization_code",
                ["redirect_uri"] = _configuration.RedirectUri
            })
        };

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return TwitchAuthenticationResponse.Invalid;
        }

        var result = await response.Content.ReadFromJsonAsync<TwitchAuthenticationResponse>();

        return result!;
    }
}
