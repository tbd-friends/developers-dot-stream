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
            var existingLabels =
                (await labelRepository.ListAsync(new LabelIdsForTagsSpec(command.Tags), cancellationToken)).ToList();

            var labelsToAdd = command.Tags
                .Where(t => existingLabels.All(l => l.Text != t))
                .Select(t => new Label { Text = t }).ToList();

            if (labelsToAdd.Any())
            {
                await labelRepository.AddRangeAsync(labelsToAdd, cancellationToken);
            }

            var labels = labelsToAdd.Union(existingLabels).ToList();

            var streamer =
                await streamerRepository.FirstOrDefaultAsync(
                    new StreamerByIdentifierWithDetailsSpec(command.UserIdentifier), cancellationToken);

            var tagsToRemove = streamer.Tags.Where(t => labels.All(l => l.Id != t.Id)).ToList();

            if (tagsToRemove.Any())
            {
                foreach (var tag in tagsToRemove)
                {
                    await tagRepository.DeleteAsync(tag, cancellationToken);
                }
            }

            var tagsToAdd = labels.Where(l => streamer.Tags.All(t => t.LabelId != l.Id)).ToList();

            foreach (var label in tagsToAdd)
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