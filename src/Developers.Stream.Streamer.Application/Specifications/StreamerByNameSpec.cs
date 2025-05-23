using Ardalis.Specification;
using Developers.Stream.Shared_Kernel;
using Developers.Stream.Streamer.Application.ViewModels;
namespace Developers.Stream.Streamer.Application.Specifications;

public sealed class StreamerByNameSpec : Specification<Domain.Streamer, StreamerViewModel>, ISingleResultSpecification<Domain.Streamer, StreamerViewModel>
{
    public StreamerByNameSpec(string name)
    {
        Query
            .Include(s => s.Channels)
            .ThenInclude(c => c.Platform)
            .Include(s => s.Tags)
            .ThenInclude(t => t.Label)
            .Where(s => s.Channels.Any(c => c.Name.Replace("@", "") == name))
            .Select(s => new StreamerViewModel
            {
                Name = s.Name,
                Blurb = s.Blurb,
                Tags = s.Tags.Select(s => s.Label.Text),
                Channels = from c in s.Channels
                           let platform = (PlatformIdentifier)c.Platform.Name
                           where c.IsVerified 
                           select new StreamerViewModel.Channel
                           {
                               Platform = c.Platform.Name,
                               Url = $"{platform.Address}{c.Name}"
                           }
            });
    }
}