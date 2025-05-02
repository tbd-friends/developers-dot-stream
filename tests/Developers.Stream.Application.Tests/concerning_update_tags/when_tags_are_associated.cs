namespace Developers.Stream.Application.Tests.concerning_update_tags;

public class when_tags_are_associated : IAsyncLifetime
{
    private readonly UpdateTags.Handler _subject;

    private readonly IRepository<Label> _labelRepository = Substitute.For<IRepository<Label>>();
    private readonly IRepository<Tag> _tagRepository = Substitute.For<IRepository<Tag>>();
    private readonly IRepository<Streamer> _streamerRepository = Substitute.For<IRepository<Streamer>>();

    private readonly Guid _streamerIdentifier = Guid.NewGuid();
    private readonly int _streamerId = 9009;
    private readonly int _existingLabelId = 6501;
    private readonly int _existingTagId = 10245;
    private readonly string _tag = "Tag";
    private readonly string _newTag = "NewTag";
    private readonly int _newLabelId = 25010;

    [Fact]
    public void new_label_is_added()
    {
        _labelRepository
            .Received(1)
            .AddRangeAsync(Arg.Is<IEnumerable<Label>>(a => a.All(l => l.Text == _newTag)));
    }

    [Fact]
    public void new_tag_is_associated()
    {
        _tagRepository
            .Received(1)
            .AddAsync(Arg.Is<Tag>(t => t.StreamerId == _streamerId && t.LabelId == _newLabelId));
    }

    [Fact]
    public void existing_tags_are_not_duplicated()
    {
        _tagRepository
            .DidNotReceive()
            .AddAsync(Arg.Is<Tag>(t => t.StreamerId == _streamerId && t.LabelId == _existingLabelId));
    }

    public when_tags_are_associated()
    {
        _labelRepository
            .When(r => r.AddRangeAsync(Arg.Is<IEnumerable<Label>>(a => a.All(l => l.Text == _newTag))))
            .Do(i => { Array.ForEach(i.Arg<IEnumerable<Label>>().ToArray(), l => l.Id = _newLabelId); });

        _labelRepository
            .ListAsync(Arg.Any<LabelIdsForTagsSpec>(), Arg.Any<CancellationToken>())
            .Returns([new Label { Id = _existingLabelId, Text = _tag }]);

        _streamerRepository
            .FirstOrDefaultAsync(Arg.Any<StreamerByIdentifierWithDetailsSpec>(), CancellationToken.None)
            .Returns(new Streamer
            {
                Id = _streamerId,
                Identifier = _streamerIdentifier,
                Tags = [
                    new Tag { Id = _existingTagId, LabelId = _existingLabelId, StreamerId = _streamerId }
                    ]
            });

        _subject = new UpdateTags.Handler(_labelRepository, _tagRepository, _streamerRepository);
    }

    public async Task InitializeAsync()
    {
        await _subject.Handle(new UpdateTags.Command(
            _streamerIdentifier, [
                _tag,
                _newTag
            ]), CancellationToken.None);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}