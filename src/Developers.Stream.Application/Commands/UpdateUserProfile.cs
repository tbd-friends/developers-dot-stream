using Ardalis.Specification;
using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Contracts;
using Mediator;

namespace Developers.Stream.Application.Commands;

public class UpdateUserProfile
{
    public record Command(Guid UserIdentifier, string Name, string Blurb) : ICommand;

    public class Handler(IRepository<Streamer> repository) : ICommandHandler<Command>
    {
        public async ValueTask<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var existing =
                await repository.FirstOrDefaultAsync(new StreamerByIdentifierSpec(command.UserIdentifier),
                    cancellationToken);

            if (existing is null)
            {
                await repository.AddAsync(new Streamer
                {
                    Identifier = command.UserIdentifier,
                    Name = command.Name,
                    Blurb = command.Blurb
                }, cancellationToken);
            }
            else
            {
                existing.Blurb = command.Blurb;
                existing.Name = command.Name;

                await repository.UpdateAsync(existing, cancellationToken);
            }

            return Unit.Value;
        }
    }

    private sealed class StreamerByIdentifierSpec : Specification<Streamer>, ISingleResultSpecification<Streamer>
    {
        public StreamerByIdentifierSpec(Guid userIdentifier)
        {
            Query.Where(s => s.Identifier == userIdentifier);
        }
    }
}