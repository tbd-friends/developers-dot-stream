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