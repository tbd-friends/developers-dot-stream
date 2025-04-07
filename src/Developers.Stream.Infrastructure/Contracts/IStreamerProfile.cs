namespace Developers.Stream.Infrastructure.Contracts;

public interface IStreamerProfile
{
    Task UpdateProfile(Guid userIdentifier, string name, string blurb, CancellationToken cancellationToken);
    Task<string> RegisterTwitchChannel(Guid userIdentifier, CancellationToken cancellation);
}