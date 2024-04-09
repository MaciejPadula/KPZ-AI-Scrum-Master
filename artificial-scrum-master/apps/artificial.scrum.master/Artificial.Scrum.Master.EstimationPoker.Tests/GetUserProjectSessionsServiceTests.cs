using Artificial.Scrum.Master.EstimationPoker.Features.GetUserProjectSessions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.EstimationPoker.Tests;

public class GetUserProjectSessionsServiceTests
{
    private GetUserProjectSessionsService _sut;
    private IUserAccessor _userAccessor;
    private ISessionRepository _sessionRepository;

    [SetUp]
    public void SetUp()
    {
        _userAccessor = Substitute.For<IUserAccessor>();
        _sessionRepository = Substitute.For<ISessionRepository>();

        _sut = new GetUserProjectSessionsService(
            _userAccessor,
            _sessionRepository);
    }

    [Test]
    public async Task Handle_WhenSessionExistsAndUserIsValidated_ReturnsValidResult()
    {
        // Arrange
        var projectId = 1;
        _userAccessor.UserId.Returns("UserId");
        _sessionRepository.GetUserProjectSessions("UserId", projectId).Returns(
        [
            new("21", "Session 1", projectId),
            new("37", "Session 2", projectId)
        ]);
        var expectedResult = new GetUserProjectSessionsResponse(
        [
            new("21", "Session 1", 1),
            new("37", "Session 2", 1)
        ]);

        // Act
        var result = await _sut.Handle(projectId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResult);
        result.Error.Should().BeNull();
    }
}
