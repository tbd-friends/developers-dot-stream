using Ardalis.Specification;
using Developers.Stream.Domain;
using Developers.Stream.Shared_Kernel.DataTransfer;

namespace Developers.Stream.Application.Specifications;

public class AllStreamersSpec : Specification<Streamer, StreamerDto>
{
    public AllStreamersSpec()
    {
        Query
            .Select(s => new StreamerDto
            {
                Id = s.Id,
                Name = s.Name,
                Blurb = s.Blurb
            });
    }
}