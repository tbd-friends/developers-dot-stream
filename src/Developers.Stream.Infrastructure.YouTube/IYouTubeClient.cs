using Ardalis.Result;

namespace Developers.Stream.Infrastructure.YouTube;

public interface IYouTubeClient
{
    Task<Result<string>> FetchChannelNameUsingAuthenticationCode(string code);
}