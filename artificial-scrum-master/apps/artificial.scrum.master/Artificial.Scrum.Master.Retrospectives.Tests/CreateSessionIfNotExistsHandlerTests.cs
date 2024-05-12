using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.Retrospectives.Features.CreateSessionIfNotExists;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using Artificial.Scrum.Master.SharedKernel;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.Retrospectives.Tests;

public class CreateSessionIfNotExistsHandlerTests
{
    private CreateSessionIfNotExistsHandler _sut;
    private IRetroSessionRepository _retroSessionRepository;
    private ISessionKeyGenerator _sessionKeyGenerator;
    private IUserAccessor _userAccessor;

    [SetUp]
    public void SetUp()
    {
        _retroSessionRepository = Substitute.For<IRetroSessionRepository>();
        _sessionKeyGenerator = Substitute.For<ISessionKeyGenerator>();
        _userAccessor = Substitute.For<IUserAccessor>();

        _sut = new CreateSessionIfNotExistsHandler(
            _retroSessionRepository,
            _sessionKeyGenerator,
            _userAccessor);
    }

    [Test]
    public async Task Handle_WhenUserIsNotAuthenticated_ReturnsUserNotAuthenticatedException()
    {
        _userAccessor.UserId.Returns((string)null!);

        var result = await _sut.Handle(new CreateSessionIfNotExistsRequest(
            "name",
            1,
            2));

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<UnauthorizedAccessException>();
        await _retroSessionRepository.Received(0).CreateSession(Arg.Any<RetroSession>());
    }

    [Test]
    public async Task Handle_WhenSessionDoesNotExist_ReturnsValidResult()
    {
        var userId = Guid.NewGuid().ToString();
        var sessionKey = Guid.NewGuid().ToString();
        var name = "name";
        var sprintId = 1;
        var projectId = 2;
        _userAccessor.UserId.Returns(userId);
        _sessionKeyGenerator.Key.Returns(sessionKey);
        _retroSessionRepository.GetSession(sprintId).Returns((RetroSession?)null);

        var result = await _sut.Handle(new CreateSessionIfNotExistsRequest(
            name,
            sprintId,
            projectId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(new CreateSessionIfNotExistsResponse(sessionKey));
        await _retroSessionRepository.Received(1).CreateSession(Arg.Any<RetroSession>());
    }

    [Test]
    public async Task Handle_WhenSessionExists_ReturnsValidResult()
    {
        var userId = Guid.NewGuid().ToString();
        var sessionKey = Guid.NewGuid().ToString();
        var name = "name";
        var sprintId = 1;
        var projectId = 2;
        _userAccessor.UserId.Returns(userId);
        _retroSessionRepository.GetSession(sprintId).Returns(new RetroSession(
            sessionKey,
            name,
            sprintId,
            projectId));

        var result = await _sut.Handle(new CreateSessionIfNotExistsRequest(
            name,
            sprintId,
            projectId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(new CreateSessionIfNotExistsResponse(sessionKey));
        await _retroSessionRepository.Received(0).CreateSession(Arg.Any<RetroSession>());
    }
}
