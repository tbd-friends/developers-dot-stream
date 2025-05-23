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
            var labels = await FetchLabelsToAssociate(command, cancellationToken);

            var streamer =
                await streamerRepository.FirstOrDefaultAsync(
                    new StreamerByIdentifierWithDetailsSpec(command.UserIdentifier), cancellationToken);

            ArgumentNullException.ThrowIfNull(streamer);

            await RemoveTagsForUnusedLabels(streamer, labels, cancellationToken);

            await AddNewTags(streamer, labels, cancellationToken);

            return Unit.Value;
        }

        private async ValueTask<List<Label>> FetchLabelsToAssociate(Command command,
            CancellationToken cancellationToken)
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
            return labels;
        }

        private async ValueTask AddNewTags(
            Streamer streamer,
            List<Label> labels,
            CancellationToken cancellationToken)
        {
            var tagsToAdd = labels.Where(l => streamer.Tags.All(t => t.LabelId != l.Id)).ToList();

            foreach (var label in tagsToAdd)
            {
                await tagRepository.AddAsync(new Tag
                {
                    StreamerId = streamer.Id,
                    LabelId = label.Id
                }, cancellationToken);
            }
        }

        private async ValueTask RemoveTagsForUnusedLabels(
            Streamer streamer,
            List<Label> labels,
            CancellationToken cancellationToken)
        {
            var tagsToRemove = streamer
                .Tags
                .Where(tag => labels.All(label => label.Id != tag.LabelId))
                .Select(t => t.Id)
                .ToList();

            if (tagsToRemove.Any())
            {
                await tagRepository.DeleteRangeAsync(new TagsByIdSpec(tagsToRemove), cancellationToken);
            }
        }
    }
}