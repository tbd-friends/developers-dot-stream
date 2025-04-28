using Developers.Stream.Application.Specifications;
using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Contracts;
using Mediator;

namespace Developers.Stream.Application.Commands;

public class UpdateTags
{
    public record Command(Guid UserIdentifier, IEnumerable<string> Tags) : ICommand;

    public class Handler(
        IRepository<Label> labelRepository,
        IRepository<Tag> tagRepository,
        IRepository<Streamer> streamerRepository) : ICommandHandler<Command>
    {
        public async ValueTask<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var labelsToAdd = command.Tags.Select(t => new Label { Text = t }).ToList();

            await labelRepository.AddRangeAsync(labelsToAdd, cancellationToken);

            var streamer =
                await streamerRepository.FirstOrDefaultAsync(
                    new StreamerByIdentifierWithDetailsSpec(command.UserIdentifier), cancellationToken);

            foreach (var label in labelsToAdd)
            {
                await tagRepository.AddAsync(new Tag
                {
                    StreamerId = streamer.Id,
                    LabelId = label.Id
                }, cancellationToken);
            }

            return Unit.Value;
        }
    }
}