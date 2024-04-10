using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionUser;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.EstimationPoker.Tests;

public class AddSessionUserServiceTests
{
    private AddSessionUserService _sut;
    private ISessionRepository _sessionRepository;
    private ISessionUserRepository _sessionUserRepository;

    [SetUp]
    public void SetUp()
    {
        _sessionRepository = Substitute.For<ISessionRepository>();
        _sessionUserRepository = Substitute.For<ISessionUserRepository>();
        _sut = new AddSessionUserService(_sessionRepository, _sessionUserRepository);
    }

    [Test]
    public async Task Handle_WhenSessionDoesNotExist_ReturnsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        var request = new AddSessionUserRequest(
            sessionId,
            "UserName");
        _sessionRepository.SessionExists(sessionId).Returns(false);

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<SessionNotFoundException>();
        await _sessionUserRepository.Received(0).AddSessionUser(Arg.Any<SessionUserEntity>());
    }

    [Test]
    public async Task Handle_WhenSessionExists_ReturnsValidResult()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        var request = new AddSessionUserRequest(sessionId, "UserName");
        _sessionRepository.SessionExists(sessionId).Returns(true);

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
        await _sessionUserRepository.Received(1).AddSessionUser(Arg.Is<SessionUserEntity>(x =>
            x.SessionId == request.SessionId
            && x.UserName == request.UserName));
    }
}
