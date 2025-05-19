using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Streamer.Api.Filters.Specifications;
namespace Developers.Stream.Streamer.Api.Filters;

public class ApiKeyFilter(IRepository<ApiKey> repository) : IEndpointFilter
{

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var apiKey) || apiKey.Count == 0)
        {
            return null;
        }

        var matches = await repository.FirstOrDefaultAsync(new MatchingApiKeySpec(apiKey!), CancellationToken.None);

        if (matches == null)
        {
            return null;
        }

        return await next(context);
    }
}