using System.Net.Http.Headers;
using System.Net.Http.Json;
using Ardalis.Result;
using Microsoft.Extensions.Options;

namespace Developers.Stream.Infrastructure.Twitch;

public class TwitchClient(
    HttpClient client,
    IOptions<TwitchConfiguration> configuration,
    TwitchChannelNameFromAuthenticationDelegate fetchChannelName
) : ITwitchClient
{
    private readonly TwitchConfiguration _configuration = configuration.Value;

    public async Task<Result<string>> FetchChannelNameUsingAuthenticationCode(string code)
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
            return Result.Error();
        }

        var twitchAuthenticationResponse = await response.Content.ReadFromJsonAsync<TwitchAuthenticationResponse>();

        if (twitchAuthenticationResponse is not { IsValid: true })
        {
            return Result.Forbidden();
        }

        var channelName = await fetchChannelName(twitchAuthenticationResponse);

        if (channelName is null)
        {
            return Result.NotFound();
        }

        return Result.Success(channelName);
    }
}