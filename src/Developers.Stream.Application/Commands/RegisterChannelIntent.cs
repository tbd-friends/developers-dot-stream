using Developers.Stream.Application.Specifications;
using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Shared_Kernel;
using Mediator;

namespace Developers.Stream.Application.Commands;

public class RegisterChannelIntent
{
    public record Command(PlatformIdentifier Platform, Guid UserIdentifier, string State) : ICommand;

    public class Handler(
        IRepository<Streamer> streamerRepository,
        IRepository<Platform> platformRepository,
        IRepository<Channel> channelRepository) : ICommandHandler<Command>
    {
        public async ValueTask<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var streamer = await streamerRepository.FirstOrDefaultAsync(
                new StreamerByIdentifierSpec(command.UserIdentifier),
                cancellationToken);

            ArgumentNullException.ThrowIfNull(streamer, nameof(streamer));

            var platform = await platformRepository.FirstOrDefaultAsync(
                new PlatformByNameSpec(command.Platform.Identifier),
                cancellationToken);

            ArgumentNullException.ThrowIfNull(platform, nameof(platform));

            await channelRepository.AddAsync(new Channel()
            {
                Name = string.Empty,
                StreamerId = streamer.Id,
                PlatformId = platform.Id,
                State = command.State,
                IsVerified = false
            }, cancellationToken);

            return Unit.Value;
        }
    }
}