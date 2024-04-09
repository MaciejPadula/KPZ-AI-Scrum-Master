using Artificial.Scrum.Master.EstimationPoker.Features.AddSessionTask;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared;
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

    [SetUp]
    public void SetUp()
    {
        _sessionRepository = Substitute.For<ISessionRepository>();
        _userAccessor = Substitute.For<IUserAccessor>();
        _sut = new AddSessionTaskService(_sessionRepository, _userAccessor);
    }

    [Test]
    [TestCase(false, false, typeof(SessionNotFoundException))]
    [TestCase(false, true, typeof(SessionNotFoundException))]
    [TestCase(true, false, typeof(UnauthorizedAccessException))]
    public async Task Handle_WhenSessionDoesNotExistOrUserIsNotValidated_ReturnsException(bool sessionExists, bool isUserValidated, Type exceptionType)
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        var request = new AddSessionTaskRequest(
            sessionId,
            "Ttitle",
            "Description");
        _sessionRepository.SessionExists(sessionId).Returns(sessionExists);
        _sessionRepository.ValidateUserAccess(_userAccessor.UserId, sessionId).Returns(isUserValidated);

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType(exceptionType);
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
        _sessionRepository.SessionExists(sessionId).Returns(true);
        _sessionRepository.ValidateUserAccess(_userAccessor.UserId, sessionId).Returns(true);

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }
}
