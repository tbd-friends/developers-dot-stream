using Ardalis.Result;
using Developers.Stream.Application.Queries.Results;
using Developers.Stream.Application.Specifications;
using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Contracts;
using Mediator;

namespace Developers.Stream.Application.Queries;

public class GetStreamerProfile
{
    public record Query(Guid UserIdentifier) : IQuery<Result<Profile>>;

    public class Handler(IRepository<Streamer> repository) : IQueryHandler<Query, Result<Profile>>
    {
        public async ValueTask<Result<Profile>> Handle(Query query, CancellationToken cancellationToken)
        {
            var streamer = await repository.SingleOrDefaultAsync(
                new StreamerByIdentifierWithDetailsSpec(query.UserIdentifier),
                cancellationToken);

            if (streamer == null)
            {
                return Result.NotFound();
            }

            return Result.Success(new Profile(
                streamer.Identifier,
                streamer.Name,
                streamer.Blurb,
                streamer.Channels.Select(s => s.Platform.Name),
                streamer.Tags.Select(t => t.Label.Text)
            ));
        }
    }
}