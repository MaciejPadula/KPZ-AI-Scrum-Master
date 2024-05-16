using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.Retrospectives.Features.GetSuggestedIdeas;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.Retrospectives.Tests;

public class GetSuggestedIdeasHandlerTests
{
    private GetSuggestedIdeasHandler _sut;
    private IUserAccessor _userAccessor;
    private IRetroSessionRepository _retroSessionRepository;
    private IRetroSuggestionService _retroSuggestionService;

    [SetUp]
    public void SetUp()
    {
        _userAccessor = Substitute.For<IUserAccessor>();
        _retroSessionRepository = Substitute.For<IRetroSessionRepository>();
        _retroSuggestionService = Substitute.For<IRetroSuggestionService>();

        _sut = new GetSuggestedIdeasHandler(
            _userAccessor,
            _retroSessionRepository,
            _retroSuggestionService);
    }

    [Test]
    public async Task Handle_WhenUserIsNotAuthenticated_ReturnsUnauthorizedAccessException()
    {
        // Arrange
        _userAccessor.UserId.Returns(string.Empty);

        // Act
        var result = await _sut.Handle("sessionId");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Test]
    public async Task Handle_WhenSessionDoesNotExist_ReturnsSessionNotFoundException()
    {
        // Arrange
        _userAccessor.UserId.Returns("userId");
        _retroSessionRepository.SessionExists("sessionId").Returns(false);

        // Act
        var result = await _sut.Handle("sessionId");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<SessionNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenSessionExists_ReturnsGetSuggestedIdeasResponse()
    {
        // Arrange
        _userAccessor.UserId.Returns("userId");
        _retroSessionRepository.SessionExists("sessionId").Returns(true);
        var cards = new List<SessionCard>
        {
            new()
            {
                Content = "content1", Type = CardType.Good
            },
            new()
            {
                Content = "content2", Type = CardType.Bad
            },
            new()
            {
                Content = "content3", Type = CardType.Ideas
            }
        };
        _retroSessionRepository.GetSessionCards("sessionId").Returns(cards);
        var suggestion = new GetSuggestedIdeasResult(["idea1", "idea2"]);
        _retroSuggestionService.GetSuggestedIdeas(cards).Returns(suggestion);

        // Act
        var result = await _sut.Handle("sessionId");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.SuggestedIdeas.Should().BeEquivalentTo(suggestion.Ideas);
    }
}
