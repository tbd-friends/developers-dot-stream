using Developers.Stream.Application.Specifications;
using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Shared_Kernel.DataTransfer;
using Mediator;

namespace Developers.Stream.Application.Queries;

public class GetStreamers
{
    public record Query : IQuery<IEnumerable<StreamerDto>>;

    public class Handler(IRepository<Streamer> repository) : IQueryHandler<Query, IEnumerable<StreamerDto>>
    {
        public async ValueTask<IEnumerable<StreamerDto>> Handle(Query query, CancellationToken cancellationToken)
        {
            var streamers = await repository.ListAsync(new ActiveStreamersWithDetailsSpec(), cancellationToken);

            return streamers;
        }
    }
}