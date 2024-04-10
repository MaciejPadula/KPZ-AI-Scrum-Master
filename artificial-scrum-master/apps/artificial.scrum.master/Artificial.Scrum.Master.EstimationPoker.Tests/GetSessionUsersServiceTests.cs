using Artificial.Scrum.Master.EstimationPoker.Features.GetSessionUsers;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.EstimationPoker.Tests;

public class GetSessionUsersServiceTests
{
    private GetSessionUsersService _sut;
    private ISessionRepository _sessionRepository;
    private ISessionUserRepository _sessionUserRepository;

    [SetUp]
    public void SetUp()
    {
        _sessionRepository = Substitute.For<ISessionRepository>();
        _sessionUserRepository = Substitute.For<ISessionUserRepository>();
        _sut = new GetSessionUsersService(_sessionRepository, _sessionUserRepository);
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
        _sessionUserRepository.GetSessionUsers(sessionId).Returns([
            new(sessionId, "User1") { Id = 21 },
            new(sessionId, "User2") { Id = 37 }]);
        var expectedResult = new GetSessionUsersResponse([
            new SessionUser(
                21, "User1"),
            new SessionUser(
                37, "User2")]);

        // Act
        var result = await _sut.Handle(sessionId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResult);
        result.Error.Should().BeNull();
    }
}
