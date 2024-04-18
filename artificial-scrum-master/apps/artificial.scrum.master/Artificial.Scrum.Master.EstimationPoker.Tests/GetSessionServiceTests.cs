using Artificial.Scrum.Master.EstimationPoker.Features.GetSession;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.EstimationPoker.Tests;

public class GetSessionServiceTests
{
    private GetSessionService _sut;
    private ISessionRepository _sessionRepository;

    [SetUp]
    public void SetUp()
    {
        _sessionRepository = Substitute.For<ISessionRepository>();
        _sut = new GetSessionService(_sessionRepository);
    }

    [Test]
    public async Task Handle_WhenSessionDoesNotExist_ReturnsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        _sessionRepository.GetSession(sessionId).Returns((SessionEntity?)null);

        // Act
        var result = await _sut.Handle(sessionId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<SessionNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenSessionExists_ReturnsValidResult()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        var session = new SessionEntity(sessionId, "Session 1", 1, "2");
        _sessionRepository.GetSession(sessionId).Returns(session);
        var expectedResult = new GetSessionResponse(new(
            sessionId, "Session 1", 1, "2"));

        // Act
        var result = await _sut.Handle(sessionId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResult);
        result.Error.Should().BeNull();
    }
}
