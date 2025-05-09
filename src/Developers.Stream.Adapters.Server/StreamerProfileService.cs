using Developers.Stream.Application.Commands;
using Developers.Stream.Application.Queries;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Infrastructure.Kick;
using Developers.Stream.Infrastructure.Twitch;
using Developers.Stream.Infrastructure.YouTube;
using Developers.Stream.Shared_Kernel;
using Developers.Stream.Shared_Kernel.Infrastructure;
using Mediator;
using Microsoft.Extensions.Options;

namespace Developers.Stream.Adapters.Server;

public class StreamerProfileService(
    ISender sender,
    IOptions<KickConfiguration> kickOptions,
    IOptions<TwitchConfiguration> twitchOptions,
    IOptions<YouTubeConfiguration> youtubeOptions) : IStreamerProfileService {

    private readonly KickConfiguration _kickConfiguration = kickOptions.Value;
    private readonly TwitchConfiguration _configuration = twitchOptions.Value;
    private readonly YouTubeConfiguration _ytConfiguration = youtubeOptions.Value;

    public async Task<string> FetchKickRegistrationLink(Guid userIdentifier, CancellationToken cancellationToken = default)
    {
        string state = new RandomString();

        await sender.Send(
        new RegisterChannelIntent.Command(PlatformIdentifier.Kick, userIdentifier, state),
        cancellationToken);

        var authUrl = _kickConfiguration.AuthUrl;
        var clientId = _kickConfiguration.ClientId;
        var redirectUri = _kickConfiguration.CodeFlowRedirectUri;

        var registrationLink =
            $"{authUrl}?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope=openid&state={state}&nonce={state}";

        return registrationLink;
    }

    public async Task<string> FetchTwitchRegistrationLink(Guid userIdentifier, CancellationToken cancellationToken = default)
    {
        string state = new RandomString();

        await sender.Send(new RegisterChannelIntent.Command(PlatformIdentifier.Twitch, userIdentifier, state),
        cancellationToken);

        var authUrl = _configuration.AuthUrl;
        var clientId = _configuration.ClientId;
        var redirectUri = _configuration.CodeFlowRedirectUri;

        var registrationLink =
            $"{authUrl}?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope=openid&state={state}&nonce={state}";

        return registrationLink;
    }

    public async Task<string> FetchYouTubeRegistrationLink(Guid userIdentifier,
        CancellationToken cancellation = default)
    {
        string state = new RandomString();

        await sender.Send(new RegisterChannelIntent.Command(PlatformIdentifier.YouTube, userIdentifier, state),
        cancellation);

        var authUrl = _ytConfiguration.AuthUrl;
        var clientId = _ytConfiguration.ClientId;
        var redirectUri = _ytConfiguration.CodeFlowRedirectUri;

        var registrationLink =
            $"{authUrl}?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope=https://www.googleapis.com/auth/youtube.readonly&state={state}&nonce={state}";

        return registrationLink;
    }

    public async Task<StreamerProfile> FetchProfile(Guid userIdentifier, CancellationToken cancellationToken = default)
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

    public async Task UpdateProfile(StreamerUpdateModel update, CancellationToken cancellationToken = default)
    {
        await sender.Send(new UpdateUserProfile.Command(
        update.Identifier,
        update.Name,
        update.Blurb),
        cancellationToken);
    }

    public async Task UpdateTags(Guid identifier, IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        await sender.Send(new UpdateTags.Command(
        identifier,
        tags), cancellationToken);
    }
}