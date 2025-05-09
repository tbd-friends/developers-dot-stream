using Developers.Stream.Shared_Kernel;

namespace Developers.Stream.Infrastructure.Contracts;

public interface IStreamerProfileService
{
    Task<StreamerProfile> FetchProfile(Guid userIdentifier, CancellationToken cancellationToken = default);
    Task UpdateProfile(StreamerUpdateModel update, CancellationToken cancellationToken = default);
    Task UpdateTags(Guid identifier, IEnumerable<string> tags, CancellationToken cancellationToken = default);
    Task<string> GenerateApiKey(Guid identifier, CancellationToken cancellationToken = default);
    Task<string> FetchKickRegistrationLink(Guid userIdentifier, CancellationToken cancellationToken = default);
    Task<string> FetchTwitchRegistrationLink(Guid userIdentifier, CancellationToken cancellationToken = default);
    Task<string> FetchYouTubeRegistrationLink(Guid userIdentifier, CancellationToken cancellationToken = default);
}

public record StreamerUpdateModel(
    Guid Identifier,
    string Name,
    string Blurb
);

public record StreamerProfile(
    Guid Identifier,
    string Name,
    string Blurb,
    IEnumerable<PlatformIdentifier> Channels,
    IEnumerable<string> Tags)
{
    public static StreamerProfile Default = new(Guid.Empty, string.Empty, string.Empty, [], []);
}