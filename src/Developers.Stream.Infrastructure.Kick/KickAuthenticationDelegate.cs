using Microsoft.IdentityModel.JsonWebTokens;
namespace Developers.Stream.Infrastructure.Kick;

public delegate Task<string?> KickChannelNameFromAuthenticationDelegate(KickAuthenticationResponse response);

public static class KickDefaults
{
    public static readonly KickChannelNameFromAuthenticationDelegate FetchChannelFromAuthenticationResponse =
        response =>
        {
            var handler = new JsonWebTokenHandler();

            var token = handler.ReadJsonWebToken(response.IdToken);

            return Task.FromResult(token.GetClaim("preferred_username")?.Value);
        };
}