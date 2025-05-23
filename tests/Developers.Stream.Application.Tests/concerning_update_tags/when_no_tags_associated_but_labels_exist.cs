namespace Developers.Stream.Application.Tests.concerning_update_tags;

public class when_no_tags_associated_but_labels_exist : IAsyncLifetime
{
    private readonly UpdateTags.Handler _subject;

    private readonly IRepository<Label> _labelRepository = Substitute.For<IRepository<Label>>();
    private readonly IRepository<Tag> _tagRepository = Substitute.For<IRepository<Tag>>();
    private readonly IRepository<Streamer> _streamerRepository = Substitute.For<IRepository<Streamer>>();

    private readonly Guid _streamerIdentifier = Guid.NewGuid();
    private readonly int _streamerId = 9009;
    private readonly int _existingLabelId = 6501;
    private readonly string _tag = "Tag";

    [Fact]
    public async Task label_is_not_created()
    {
        await _labelRepository
            .DidNotReceive()
            .AddRangeAsync(Arg.Any<IEnumerable<Label>>());
    }

    [Fact]
    public async Task tag_for_label_is_associated_with_streamer()
    {
        await _tagRepository
            .Received(1)
            .AddAsync(Arg.Is<Tag>(l => l.StreamerId == _streamerId && l.LabelId == _existingLabelId),
                Arg.Any<CancellationToken>());
    }

    public when_no_tags_associated_but_labels_exist()
    {
        _labelRepository
            .ListAsync(Arg.Any<LabelIdsForTagsSpec>(), Arg.Any<CancellationToken>())
            .Returns([new Label { Id = _existingLabelId, Text = _tag }]);

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
        await _subject.Handle(new UpdateTags.Command(
            _streamerIdentifier, [
                _tag
            ]), CancellationToken.None);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}