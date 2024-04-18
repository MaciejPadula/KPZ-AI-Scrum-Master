using Artificial.Scrum.Master.EstimationPoker.Features.CreateSession;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.EstimationPoker.Tests;

public class CreateSessionServiceTests
{
    private CreateSessionService _sut;
    private ISessionRepository _sessionRepository;
    private ISessionKeyGenerator _sessionKeyGenerator;
    private IUserAccessor _userAccessor;

    [SetUp]
    public void SetUp()
    {
        _sessionRepository = Substitute.For<ISessionRepository>();
        _sessionKeyGenerator = Substitute.For<ISessionKeyGenerator>();
        _userAccessor = Substitute.For<IUserAccessor>();

        _sut = new CreateSessionService(
            _sessionRepository,
            _sessionKeyGenerator,
            _userAccessor);
    }

    [Test]
    public async Task Handle_WhenUserIsNotAuthenticated_ReturnsException()
    {
        // Arrange
        _userAccessor.UserId.Returns((string?)null);

        // Act
        var result = await _sut.Handle(new(1, ""));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<UserNotAuthenticatedException>();
        await _sessionRepository.Received(0).AddSession(Arg.Any<SessionEntity>());
    }

    [Test]
    public async Task Handle_WhenUserIsAuthenticated_ReturnsValidResult()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var sessionKey = Guid.NewGuid().ToString();
        int projectId = 2137;
        var name = "papaj";
        _userAccessor.UserId.Returns(userId);
        _sessionKeyGenerator.Key.Returns(sessionKey);
        var expectedResult = new CreateSessionResponse(sessionKey);

        // Act
        var result = await _sut.Handle(new(projectId, name));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResult);
        result.Error.Should().BeNull();
        await _sessionRepository.Received(1).AddSession(Arg.Is<SessionEntity>(x =>
            x.Id == sessionKey
            && x.Name == name
            && x.ProjectId == projectId
            && x.OwnerId == userId));
    }
}
