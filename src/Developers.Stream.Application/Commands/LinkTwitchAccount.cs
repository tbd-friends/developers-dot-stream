using Developers.Stream.Application.Specifications;
using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Infrastructure.Twitch;
using Mediator;

namespace Developers.Stream.Application.Commands;

public class LinkTwitchAccount
{
    public record Command(string Code, string State) : ICommand;

    public class Handler(
        ITwitchClient client,
        IRepository<Channel> repository,
        TwitchChannelNameFromAuthenticationDelegate fetchChannelName) : ICommandHandler<Command>
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

            var authentication = await client.FetchAuthenticationFromCode(command.Code);
            
            if (!authentication.IsValid)
            {
                return Unit.Value;
            }

            var channelName = await fetchChannelName(authentication);

            if (channelName is null)
            {
                return Unit.Value;
            }

            channel.IsVerified = true;
            channel.State = string.Empty;
            channel.Name = channelName;

            await repository.UpdateAsync(channel, cancellationToken);

            return Unit.Value;
        }
    }
}