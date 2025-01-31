using Developers.Stream.Shared_Kernel.DataTransfer;

namespace Developers.Stream.Infrastructure.Contracts;

public interface IStreamerQuery
{
    Task<IEnumerable<StreamerDto>> GetStreamers();
}