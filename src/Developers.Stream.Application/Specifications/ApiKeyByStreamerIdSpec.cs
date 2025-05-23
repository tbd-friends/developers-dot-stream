using Ardalis.Specification;
using Developers.Stream.Domain;
namespace Developers.Stream.Application.Specifications;

public sealed class ApiKeyByStreamerIdSpec : Specification<ApiKey>, ISingleResultSpecification<ApiKey>
{
    public ApiKeyByStreamerIdSpec(int id)
    {
        Query
            .Where(a => a.StreamerId == id);
    }
}