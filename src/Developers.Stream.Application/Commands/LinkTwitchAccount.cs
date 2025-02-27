using Developers.Stream.Infrastructure.Twitch;
using Mediator;

namespace Developers.Stream.Application.Commands;

public class LinkTwitchAccount
{
    public record Command(string Code) : ICommand;

    public class Handler(ITwitchClient client) : ICommandHandler<Command>
    {
        public async ValueTask<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var authentication = await client.FetchAuthenticationFromCode(command.Code);

            return Unit.Value;
        }
    }
}