using Ardalis.Specification;
using Developers.Stream.Domain;

namespace Developers.Stream.Application.Specifications;

public sealed class UnverifiedChannelByStateSpec : Specification<Channel>, ISingleResultSpecification<Channel>
{
    public UnverifiedChannelByStateSpec(string state)
    {
        Query.Where(s => s.State == state && !s.IsVerified);
    }
}