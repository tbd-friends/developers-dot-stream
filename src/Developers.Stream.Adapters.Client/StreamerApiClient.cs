using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Shared_Kernel.DataTransfer;

namespace Developers.Stream.Adapters.Client;

public class StreamerApiClient(HttpClient client) : IStreamerQuery
{   
    public Task<IEnumerable<StreamerDto>> GetStreamers()
    {
        throw new NotImplementedException();
    }
}