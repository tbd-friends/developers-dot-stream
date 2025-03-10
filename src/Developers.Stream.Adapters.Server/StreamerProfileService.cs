using Developers.Stream.Application.Commands;
using Developers.Stream.Infrastructure.Contracts;
using Mediator;

namespace Developers.Stream.Adapters.Server;

public class StreamerProfileService(ISender sender) : IStreamerProfile
{
    public async Task UpdateProfile(Guid userIdentifier, string name, string blurb, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateUserProfile.Command(userIdentifier, name, blurb), cancellationToken);
    }
}