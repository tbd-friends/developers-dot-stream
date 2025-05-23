using Ardalis.Specification;
using Developers.Stream.Domain;
namespace Developers.Stream.Streamer.Api.Filters.Specifications;

public sealed class MatchingApiKeySpec : Specification<ApiKey>, ISingleResultSpecification<ApiKey>
{
    public MatchingApiKeySpec(string apiKey)
    {
        Query.Where(q => q.Key == apiKey);
    }
}