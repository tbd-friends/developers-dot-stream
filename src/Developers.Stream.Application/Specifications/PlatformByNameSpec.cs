using Ardalis.Specification;
using Developers.Stream.Domain;

namespace Developers.Stream.Application.Specifications;

public sealed class PlatformByNameSpec : Specification<Platform>, ISingleResultSpecification<Platform>
{
    public PlatformByNameSpec(string name)
    {
        Query.Where(p => p.Name == name);
    }
}