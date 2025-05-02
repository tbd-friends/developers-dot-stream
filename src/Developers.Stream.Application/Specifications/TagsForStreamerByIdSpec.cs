using Ardalis.Specification;
using Developers.Stream.Domain;

namespace Developers.Stream.Application.Specifications;

public sealed class TagsForStreamerByIdSpec : Specification<Tag>
{
    public TagsForStreamerByIdSpec(int streamerId)
    {
        Query
            .Include(t => t.Label)
            .Where(s => s.StreamerId == streamerId);
    }
}