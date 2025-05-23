namespace Developers.Stream.Application.Queries.Results;

public record Profile(
    Guid Identifier,
    string Name,
    string Blurb,
    IEnumerable<string> Channels,
    IEnumerable<string> Tags);