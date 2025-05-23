using Ardalis.Specification;
using Developers.Stream.Domain;

namespace Developers.Stream.Application.Specifications;

public sealed class TagsByIdSpec : Specification<Tag>
{
    public TagsByIdSpec(IEnumerable<int> ids)
    {
        Query
            .Where(t => ids.Contains(t.Id));
    }
}