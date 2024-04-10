using Artificial.Scrum.Master.EstimationPoker.Features.GetCurrentTask;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.EstimationPoker.Tests;

public class GetCurrentTaskServiceTests
{
    private GetCurrentTaskService _sut;
    private ISessionRepository _sessionRepository;
    private ISessionTaskRepository _sessionTaskRepository;

    [SetUp]
    public void SetUp()
    {
        _sessionRepository = Substitute.For<ISessionRepository>();
        _sessionTaskRepository = Substitute.For<ISessionTaskRepository>();
        _sut = new GetCurrentTaskService(_sessionRepository, _sessionTaskRepository);
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
    public async Task Handle_WhenSessionExistsAndNoTasks_ReturnsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        _sessionRepository.SessionExists(sessionId).Returns(true);
        _sessionTaskRepository.GetLatestTask(sessionId).Returns((SessionTaskEntity?)null);

        // Act
        var result = await _sut.Handle(sessionId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<NoTasksInSessionException>();
    }

    [Test]
    public async Task Handle_WhenSessionExistsAndTaskExist_ReturnsValidResult()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        _sessionRepository.SessionExists(sessionId).Returns(true);
        _sessionTaskRepository.GetLatestTask(sessionId).Returns(new SessionTaskEntity(sessionId, "Title", "Description", DateTime.MinValue) { Id = 1 });
        var expectedResult = new GetCurrentTaskResponse(1, "Title", "Description");

        // Act
        var result = await _sut.Handle(sessionId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResult);
        result.Error.Should().BeNull();
    }
}
