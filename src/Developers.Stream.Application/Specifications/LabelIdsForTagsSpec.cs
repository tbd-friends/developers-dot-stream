using Ardalis.Specification;
using Developers.Stream.Domain;

namespace Developers.Stream.Application.Specifications;

public sealed class LabelIdsForTagsSpec : Specification<Label>
{
    public LabelIdsForTagsSpec(IEnumerable<string> tags)
    {
        Query
            .Where(l => tags.Contains(l.Text));
    }
}