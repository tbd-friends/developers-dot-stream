namespace Developers.Stream.Streamer.Api.Filters;

public class ApiKeyFilter : IEndpointFilter
{

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var apiKey))
        {
            await next(context);
        }

        return null;
    }
}