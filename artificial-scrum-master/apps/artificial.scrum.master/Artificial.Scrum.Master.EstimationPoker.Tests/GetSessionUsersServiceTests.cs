using Artificial.Scrum.Master.EstimationPoker.Features.GetSessionUsers;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.EstimationPoker.Tests;

public class GetSessionUsersServiceTests
{
    private GetSessionUsersService _sut;
    private ISessionRepository _sessionRepository;

    [SetUp]
    public void SetUp()
    {
        _sessionRepository = Substitute.For<ISessionRepository>();
        _sut = new GetSessionUsersService(_sessionRepository);
    }

    [Test]
    public async Task Handle_WhenSessionDoesNotExist_ReturnsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        _sessionRepository.SessionExists(sessionId).Returns(false);

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
        _sessionRepository.SessionExists(sessionId).Returns(true);
        _sessionRepository.GetSessionUsers(sessionId).Returns([
            new(sessionId, "User1"),
            new(sessionId, "User2")]);
        var expectedResult = new GetSessionUsersResponse([
            new SessionUser(
                "User1"),
            new SessionUser(
                "User2")]);

        // Act
        var result = await _sut.Handle(sessionId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResult);
        result.Error.Should().BeNull();
    }
}
