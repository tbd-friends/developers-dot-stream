using Ardalis.Result;
using Developers.Stream.Application.Specifications;
using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Contracts;
using Mediator;

namespace Developers.Stream.Application.Commands;

public class RegisterApiKey
{
    public record Command(Guid UserIdentifier) : ICommand<Result<string>>;

    public class Handler(
        IRepository<ApiKey> repository,
        IRepository<Streamer> streamerRepository
    ) : ICommandHandler<Command, Result<string>>
    {
        public async ValueTask<Result<string>> Handle(Command command, CancellationToken cancellationToken)
        {
            var streamer = await streamerRepository.FirstOrDefaultAsync(new StreamerByIdentifierSpec(command.UserIdentifier), cancellationToken);

            ArgumentNullException.ThrowIfNull(streamer);

            var existingApiKey = await repository.FirstOrDefaultAsync(new ApiKeyByStreamerIdSpec(streamer.Id), cancellationToken);

            if (existingApiKey is not null)
            {
                return Result.Success(existingApiKey.Key);
            }

            var newApiKey = new ApiKey
            {
                StreamerId = streamer.Id,
                Key = Guid.NewGuid().ToString("N"),
                DateAdded = DateTime.UtcNow
            };

            await repository.AddAsync(newApiKey, cancellationToken);

            return Result.Success(newApiKey.Key);
        }
    }
}