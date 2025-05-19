using Ardalis.Result;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Streamer.Application.Specifications;
using Developers.Stream.Streamer.Application.ViewModels;
using Mediator;

namespace Developers.Stream.Streamer.Application;

public class StreamerByName
{
    public record Query(string Username) : IQuery<Result<StreamerViewModel>>;

    public class QueryHandler(IRepository<Domain.Streamer> repository) : IQueryHandler<Query, Result<StreamerViewModel>>
    {
        public async ValueTask<Result<StreamerViewModel>> Handle(Query query, CancellationToken cancellationToken)
        {
            var streamer = await repository.FirstOrDefaultAsync(new StreamerByNameSpec(query.Username), cancellationToken);

            return streamer is not null ? Result.Success(streamer) : Result.NotFound();
        }
    }
}