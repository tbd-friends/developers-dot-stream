using Ardalis.Specification;
using Developers.Stream.Domain;

namespace Developers.Stream.Application.Specifications;

public sealed class StreamerByIdentifierSpec : Specification<Streamer>, ISingleResultSpecification<Streamer>
{
    public StreamerByIdentifierSpec(Guid identifier)
    {
        Query.Where(s => s.Identifier == identifier);
    }
}

public sealed class StreamerByIdentifierWithDetailsSpec : Specification<Streamer>, ISingleResultSpecification<Streamer>
{
    public StreamerByIdentifierWithDetailsSpec(Guid identifier)
    {
        Query
            .Include(s => s.Channels)
            .ThenInclude(c => c.Platform)
            .Include(s => s.Tags)
            .Where(s => s.Identifier == identifier)
            .AsNoTracking();
    }
}