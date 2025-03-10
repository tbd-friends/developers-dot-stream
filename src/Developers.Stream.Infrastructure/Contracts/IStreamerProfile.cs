namespace Developers.Stream.Infrastructure.Contracts;

public interface IStreamerProfile
{
    Task UpdateProfile(Guid userIdentifier, string name, string blurb, CancellationToken cancellationToken);
}