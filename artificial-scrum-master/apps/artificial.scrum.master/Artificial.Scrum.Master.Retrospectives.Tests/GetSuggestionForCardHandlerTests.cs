using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.Retrospectives.Features.GetSuggestionForCard;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.Retrospectives.Tests;

public class GetSuggestionForCardHandlerTests
{
    private GetSuggestionForCardHandler _sut;
    private IUserAccessor _userAccessor;
    private IRetroSuggestionService _retroSuggestionService;

    [SetUp]
    public void Setup()
    {
        _userAccessor = Substitute.For<IUserAccessor>();
        _retroSuggestionService = Substitute.For<IRetroSuggestionService>();

        _sut = new GetSuggestionForCardHandler(
            _userAccessor,
            _retroSuggestionService);
    }

    [Test]
    public async Task Handle_WhenUserIsNotAuthenticated_ReturnsUnauthorizedAccessException()
    {
        // Arrange
        _userAccessor.UserId.Returns(string.Empty);
        var request = new GetSuggestionForCardRequest(
            "Test",
            CardType.Good);

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Test]
    public async Task Handle_WhenCardExists_ReturnsSuggestion()
    {
        // Arrange
        _userAccessor.UserId.Returns("userId");
        var request = new GetSuggestionForCardRequest(
            "Test",
            CardType.Good);
        var card = new SessionCard
        {
            Type = CardType.Good,
            Content = "Test"
        };
        _retroSuggestionService
            .GetSuggestedIdeas(Arg.Is<IEnumerable<SessionCard>>(x => x.SequenceEqual(new List<SessionCard> { card })))
            .Returns(Task.FromResult((GetSuggestedIdeasResult?)new GetSuggestedIdeasResult(["idea"])));

        var expectedResponse = new GetSuggestionForCardResponse(["idea"]);

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
