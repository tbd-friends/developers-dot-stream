using Developers.Stream.Application.Commands;
using Developers.Stream.Application.Queries;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Infrastructure.Twitch;
using Developers.Stream.Shared_Kernel;
using Developers.Stream.Shared_Kernel.Infrastructure;
using Mediator;
using Microsoft.Extensions.Options;

namespace Developers.Stream.Adapters.Server;

public class StreamerProfileService(
    ISender sender,
    IOptions<TwitchConfiguration> twitchOptions) : IStreamerProfileService
{
    private readonly TwitchConfiguration _configuration = twitchOptions.Value;

    public async Task<string> FetchTwitchRegistrationLink(Guid userIdentifier, CancellationToken cancellation)
    {
        string state = new RandomString();

        await sender.Send(new RegisterChannelIntent.Command(PlatformIdentifier.Twitch, userIdentifier, state),
            cancellation);

        var authUrl = _configuration.AuthUrl;
        var clientId = _configuration.ClientId;
        var redirectUri = _configuration.CodeFlowRedirectUri;

        var registrationLink =
            $"{authUrl}?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope=openid&state={state}&nonce={state}";

        return registrationLink;
    }

    public async Task<StreamerProfile> FetchProfile(Guid userIdentifier, CancellationToken cancellationToken)
    {
        var profile = await sender.Send(new GetStreamerProfile.Query(userIdentifier), cancellationToken);

        if (!profile.IsSuccess)
        {
            return StreamerProfile.Default;
        }

        return new StreamerProfile(
            profile.Value.Identifier,
            profile.Value.Name,
            profile.Value.Blurb,
            profile.Value.Channels.Select(c => (PlatformIdentifier)c),
            profile.Value.Tags
        );
    }

    public async Task UpdateProfile(Guid userIdentifier, string name, string blurb, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateUserProfile.Command(userIdentifier, name, blurb), cancellationToken);
    }
}