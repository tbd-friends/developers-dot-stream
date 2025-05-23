using Ardalis.Result;
namespace Developers.Stream.Infrastructure.Kick;

public interface IKickClient
{
    Task<Result<string>> FetchChannelNameUsingAuthenticationCode(string code);
}