using Developers.Stream.Application.Commands;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Shared_Kernel;
using Developers.Stream.Shared_Kernel.Infrastructure;
using Mediator;

namespace Developers.Stream.Adapters.Server;

public class StreamerProfileService(ISender sender) : IStreamerProfile
{
    public async Task<string> RegisterTwitchChannel(Guid userIdentifier, CancellationToken cancellation)
    {
        string state = new RandomString();

        await sender.Send(new RegisterChannelIntent.Command(PlatformIdentifier.Twitch, userIdentifier, state), cancellation);

        return state;
    }

    public async Task UpdateProfile(Guid userIdentifier, string name, string blurb, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateUserProfile.Command(userIdentifier, name, blurb), cancellationToken);
    }
}