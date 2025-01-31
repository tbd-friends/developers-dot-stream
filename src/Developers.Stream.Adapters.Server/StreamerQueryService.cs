using Developers.Stream.Application.Queries;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Shared_Kernel.DataTransfer;
using Mediator;

namespace Developers.Stream.Adapters.Server;

public class StreamerQueryService(ISender sender) : IStreamerQuery
{
    public async Task<IEnumerable<StreamerDto>> GetStreamers()
    {
        return await sender.Send(new GetLiveStreamers.Query());
    }
}