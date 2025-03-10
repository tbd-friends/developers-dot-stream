using Developers.Stream.Shared_Kernel;

namespace Developers.Stream.Application.Commands;

public class RegisterChannelIntent
{
    public record Command(PlatformIdentifier Platform, string UserIdentifier, string State);
}