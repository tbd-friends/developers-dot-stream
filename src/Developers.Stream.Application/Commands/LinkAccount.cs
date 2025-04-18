using Developers.Stream.Application.Specifications;
using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Infrastructure.Twitch;
using Developers.Stream.Infrastructure.YouTube;
using Developers.Stream.Shared_Kernel;
using Mediator;

namespace Developers.Stream.Application.Commands;

public class LinkAccount
{
    public record Command(string Code, string State) : ICommand;

    public class Handler(
        ITwitchClient client,
        IYouTubeClient ytClient,
        IRepository<Channel> repository) : ICommandHandler<Command>
    {
        public async ValueTask<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var channel =
                await repository.SingleOrDefaultAsync(new UnverifiedChannelByStateSpec(command.State),
                    cancellationToken);

            if (channel is null)
            {
                return Unit.Value;
            }

            string channelName = string.Empty;

            if (channel.Platform.Name == PlatformIdentifier.Twitch)
            {
                channelName = await client.FetchChannelNameUsingAuthenticationCode(command.Code);
            }

            if (channel.Platform.Name == PlatformIdentifier.YouTube)
            {
                channelName = await ytClient.FetchChannelNameUsingAuthenticationCode(command.Code);
            }

            channel.IsVerified = true;
            channel.State = string.Empty;
            channel.Name = channelName;

            await repository.UpdateAsync(channel, cancellationToken);


            return Unit.Value;
        }
    }
}