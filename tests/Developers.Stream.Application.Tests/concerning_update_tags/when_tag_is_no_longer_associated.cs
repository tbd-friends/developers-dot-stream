namespace Developers.Stream.Application.Tests.concerning_update_tags;

public class when_tag_is_no_longer_associated : IAsyncLifetime
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

    [Fact]
    public void tag_is_removed()
    {
        _tagRepository
            .Received()
            .DeleteAsync(Arg.Is<Tag>(t => t.LabelId == _existingLabelId), Arg.Any<CancellationToken>());
    }

    public when_tag_is_no_longer_associated()
    {
        _labelRepository
            .ListAsync(Arg.Any<LabelIdsForTagsSpec>(), Arg.Any<CancellationToken>())
            .Returns([]);

        _streamerRepository
            .FirstOrDefaultAsync(Arg.Any<StreamerByIdentifierWithDetailsSpec>(), CancellationToken.None)
            .Returns(new Streamer
            {
                Id = _streamerId,
                Identifier = _streamerIdentifier,
                Tags =
                [
                    new Tag
                    {
                        Id = _existingTagId,
                        LabelId = _existingLabelId,
                        StreamerId = _streamerId
                    }
                ]
            });

        _subject = new UpdateTags.Handler(_labelRepository, _tagRepository, _streamerRepository);
    }

    public async Task InitializeAsync()
    {
        await _subject.Handle(new UpdateTags.Command(
            _streamerIdentifier, []), CancellationToken.None);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}