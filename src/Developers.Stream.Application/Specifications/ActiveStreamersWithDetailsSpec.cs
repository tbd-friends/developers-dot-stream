using Ardalis.Specification;
using Developers.Stream.Domain;
using Developers.Stream.Shared_Kernel.DataTransfer;

namespace Developers.Stream.Application.Specifications;

public class ActiveStreamersWithDetailsSpec : Specification<Streamer, StreamerDto>
{
    public ActiveStreamersWithDetailsSpec()
    {
        Query
            .Where(s => s.Channels.Any(c => c.IsVerified))
            .Select(s => new StreamerDto
            {
                Id = s.Id,
                Name = s.Name,
                Blurb = s.Blurb,
                Tags = from t in s.Tags
                    select t.Label.Text,
                Channels = from c in s.Channels
                    where c.IsVerified
                    select new ChannelDto
                    {
                        Platform = c.Platform.Name,
                        Name = c.Name
                    }
            });
    }
}