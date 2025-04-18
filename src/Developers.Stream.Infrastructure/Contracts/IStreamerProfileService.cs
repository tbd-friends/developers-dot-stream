using Developers.Stream.Shared_Kernel;

namespace Developers.Stream.Infrastructure.Contracts;

public interface IStreamerProfileService
{
    Task<StreamerProfile> FetchProfile(Guid userIdentifier, CancellationToken cancellationToken);
    Task UpdateProfile(Guid userIdentifier, string name, string blurb, CancellationToken cancellationToken);
    Task<string> FetchTwitchRegistrationLink(Guid userIdentifier, CancellationToken cancellation);
    Task<string> FetchYouTubeRegistrationLink(Guid userIdentifier, CancellationToken cancellation);
}

public record StreamerProfile(
    Guid Identifier,
    string Name,
    string Blurb,
    IEnumerable<PlatformIdentifier> Channels,
    IEnumerable<string> Tags)
{
    public static StreamerProfile Default = new(Guid.Empty, string.Empty, string.Empty, [], []);
}