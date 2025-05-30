using Ardalis.Specification;
using Developers.Stream.Domain;
using Developers.Stream.Shared_Kernel.DataTransfer;

namespace Developers.Stream.Application.Specifications;

public class ActiveStreamersWithDetailsSpec : Specification<Streamer, StreamerDto>
{
    public ActiveStreamersWithDetailsSpec(string searchTerm)
    {
        Query
            .Include(s => s.Tags)
            .ThenInclude(t => t.Label)
            .Include(s => s.Channels)
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

        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            
            Query.Where(s => s.Channels.Any(c => c.IsVerified) &&
                             (s.Name.Contains(searchTerm) || s.Blurb.ToLower().Contains(searchTerm) ||
                                                              s.Tags.Any(t => t.Label.Text.ToLower().Contains(searchTerm)) ||
                                                              s.Channels.Any(c => c.Name.ToLower().Contains(searchTerm))));
        }
        else
        {
            Query.Where(s => s.Channels.Any(c => c.IsVerified));
        }
    }
}