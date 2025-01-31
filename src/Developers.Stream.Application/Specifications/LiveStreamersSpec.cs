using Ardalis.Specification;
using Developers.Stream.Domain;
using Developers.Stream.Shared_Kernel.DataTransfer;

namespace Developers.Stream.Application.Specifications;

public class LiveStreamersSpec : Specification<Streamer, StreamerDto>
{
    public LiveStreamersSpec()
    {
        Query
            .Select(s => new StreamerDto
            {
                Id = s.Id,
                Name = s.Name,
                Blurb = s.Blurb
            })
            .Include(s => s.Channels)
            .Where(s => s.Channels.Any(c => !string.IsNullOrEmpty(c.Url)));

    }
}