using Developers.Stream.Application.Commands;
using Developers.Stream.Application.Specifications;
using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Contracts;
using NSubstitute;

namespace Developers.Stream.Application.Tests.concerning_update_tags;

public class when_no_tags_are_associated : IAsyncLifetime
{
    private readonly UpdateTags.Handler _subject;

    private readonly IRepository<Label> _labelRepository = Substitute.For<IRepository<Label>>();
    private readonly IRepository<Tag> _tagRepository = Substitute.For<IRepository<Tag>>();
    private readonly IRepository<Streamer> _streamerRepository = Substitute.For<IRepository<Streamer>>();

    private readonly Guid _streamerIdentifier = Guid.NewGuid();
    private readonly int _streamerId = 9009;
    private readonly int _newLabelId = 4045;
    private readonly string _tag = "Tag";

    [Fact]
    public async Task label_is_created_for_new_tag()
    {
        await _labelRepository
            .Received(1)
            .AddRangeAsync(Arg.Is<IEnumerable<Label>>(p => p.All(l => l.Text == _tag)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task tag_is_associated_with_streamer()
    {
        await _tagRepository
            .Received(1)
            .AddAsync(Arg.Is<Tag>(l => l.StreamerId == _streamerId && l.LabelId == _newLabelId),
                Arg.Any<CancellationToken>());
    }

    public when_no_tags_are_associated()
    {
        _labelRepository
            .When(r => r.AddRangeAsync(Arg.Is<IEnumerable<Label>>(p => p.All(l => l.Text == _tag))))
            .Do(info => info.Arg<IEnumerable<Label>>().First().Id = _newLabelId);

        _streamerRepository
            .FirstOrDefaultAsync(Arg.Any<StreamerByIdentifierWithDetailsSpec>(), CancellationToken.None)
            .Returns(new Streamer
            {
                Id = _streamerId,
                Identifier = _streamerIdentifier
            });

        _subject = new UpdateTags.Handler(_labelRepository, _tagRepository, _streamerRepository);
    }

    public async Task InitializeAsync()
    {
        await _subject.Handle(new UpdateTags.Command(_streamerIdentifier, [
            _tag
        ]), CancellationToken.None);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}