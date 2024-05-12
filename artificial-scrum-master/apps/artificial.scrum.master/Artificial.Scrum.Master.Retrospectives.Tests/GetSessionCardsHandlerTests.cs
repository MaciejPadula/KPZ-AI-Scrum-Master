using Artificial.Scrum.Master.Retrospectives.Features.GetSessionCards;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.Retrospectives.Tests;

public class GetSessionCardsHandlerTests
{
    private GetSessionCardsHandler _sut;
    private IRetroSessionRepository _retroSessionRepository;

    [SetUp]
    public void SetUp()
    {
        _retroSessionRepository = Substitute.For<IRetroSessionRepository>();
        _sut = new GetSessionCardsHandler(_retroSessionRepository);
    }

    [Test]
    public async Task Handle_WhenSessionDoesNotExist_ShouldReturnSessionNotFoundException()
    {
        var sessionId = Guid.NewGuid().ToString();
        _retroSessionRepository.SessionExists(sessionId).Returns(false);

        var result = await _sut.Handle(sessionId);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<SessionNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenSessionExists_ShouldReturnCards()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        var cards = new List<SessionCard>
        {
            new("content1", CardType.Good, sessionId, new DateTime(2024, 1, 3)),
            new("content2", CardType.Bad, sessionId, new DateTime(2024, 1, 2))
        };
        _retroSessionRepository.SessionExists(sessionId).Returns(true);
        _retroSessionRepository.GetSessionCards(sessionId).Returns(cards);
        var expectedResponse = new GetSessionCardsResponse(
            ["content1"],
            ["content2"],
            []);

        // Act
        var result = await _sut.Handle(sessionId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().BeEquivalentTo(expectedResponse);
    }
}
