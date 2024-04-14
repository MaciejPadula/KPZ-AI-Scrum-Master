using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionTask;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.EstimationPoker.Tests;

public class AddSessionTaskServiceTests
{
    private AddSessionTaskService _sut;
    private ISessionRepository _sessionRepository;
    private IUserAccessor _userAccessor;
    private TimeProvider _timeProvider;
    private ISessionTaskRepository _sessionTaskRepository;

    [SetUp]
    public void SetUp()
    {
        _sessionRepository = Substitute.For<ISessionRepository>();
        _userAccessor = Substitute.For<IUserAccessor>();
        _timeProvider = Substitute.For<TimeProvider>();
        _sessionTaskRepository = Substitute.For<ISessionTaskRepository>();
        _sut = new AddSessionTaskService(_sessionRepository, _userAccessor, _timeProvider, _sessionTaskRepository);
    }

    [Test]
    [TestCase(false, false, typeof(SessionNotFoundException))]
    [TestCase(false, true, typeof(SessionNotFoundException))]
    [TestCase(true, false, typeof(UserNotAuthorizedException))]
    public async Task Handle_WhenSessionDoesNotExistOrUserIsNotValidated_ReturnsException(bool sessionExists, bool isUserValidated, Type exceptionType)
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        var request = new AddSessionTaskRequest(
            sessionId,
            "Ttitle",
            "Description");
        var userId = "123";
        _sessionRepository.SessionExists(sessionId).Returns(sessionExists);
        _sessionRepository.ValidateUserAccess(userId, sessionId).Returns(isUserValidated);
        _userAccessor.UserId.Returns(userId);

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType(exceptionType);
        await _sessionTaskRepository.Received(0).AddSessionTask(Arg.Any<SessionTaskEntity>());
    }

    [Test]
    public async Task Handle_WhenSessionExistsAndUserIsValidated_ReturnsValidResult()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        var request = new AddSessionTaskRequest(
            sessionId,
            "Ttitle",
            "Description");
        var userId = "123";
        _sessionRepository.SessionExists(sessionId).Returns(true);
        _sessionRepository.ValidateUserAccess(userId, sessionId).Returns(true);
        _userAccessor.UserId.Returns(userId);

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
        await _sessionTaskRepository.Received(1).AddSessionTask(Arg.Is<SessionTaskEntity>(x =>
            x.SessionId == sessionId
            && x.Title == request.Title
            && x.Description == request.Description));
    }
}
