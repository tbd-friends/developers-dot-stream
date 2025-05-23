using Microsoft.IdentityModel.JsonWebTokens;

namespace Developers.Stream.Infrastructure.Twitch;

public delegate Task<string?> TwitchChannelNameFromAuthenticationDelegate(TwitchAuthenticationResponse response);

public static class TwitchDefaults
{
    public static readonly TwitchChannelNameFromAuthenticationDelegate FetchChannelFromAuthenticationResponse =
        response =>
        {
            var handler = new JsonWebTokenHandler();

            var token = handler.ReadJsonWebToken(response.IdToken);

            return Task.FromResult(token.GetClaim("preferred_username")?.Value);
        };
}