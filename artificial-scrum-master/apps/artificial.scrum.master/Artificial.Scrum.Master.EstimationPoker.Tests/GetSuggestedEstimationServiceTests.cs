using Artificial.Scrum.Master.EstimationPoker.Features.GetSuggestedEstimation;
using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.EstimationPoker.Tests;

public class GetSuggestedEstimationServiceTests
{
    private GetSuggestedEstimationService _sut;
    private ISessionRepository _sessionRepository;
    private ISessionTaskRepository _sessionTaskRepository;
    private IUserAccessor _userAccessor;
    private IPokerSuggestionService _pokerSuggestionService;

    [SetUp]
    public void SetUp()
    {
        _sessionRepository = Substitute.For<ISessionRepository>();
        _sessionTaskRepository = Substitute.For<ISessionTaskRepository>();
        _userAccessor = Substitute.For<IUserAccessor>();
        _pokerSuggestionService = Substitute.For<IPokerSuggestionService>();

        _sut = new GetSuggestedEstimationService(
            _pokerSuggestionService,
            _sessionTaskRepository,
            _userAccessor,
            _sessionRepository);
    }

    [Test]
    public async Task Handle_WhenUserIsNotAuthenticated_ReturnsUnauthorizedAccessException()
    {
        // Arrange
        _userAccessor.UserId.Returns(string.Empty);

        // Act
        var result = await _sut.Handle(new GetSuggestedEstimationRequest(1, new List<decimal>()));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Exception.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Test]
    public async Task Handle_WhenTaskDoesNotExist_ReturnsTaskNotFoundException()
    {
        // Arrange
        _userAccessor.UserId.Returns("userId");
        _sessionTaskRepository.GetTaskById(1).Returns((SessionTaskEntity?)null);

        // Act
        var result = await _sut.Handle(new GetSuggestedEstimationRequest(1, new List<decimal>()));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Exception.Should().BeOfType<TaskNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenUserIsNotAuthorized_ReturnsUserNotAuthorizedException()
    {
        // Arrange
        _userAccessor.UserId.Returns("userId");
        _sessionTaskRepository.GetTaskById(1).Returns(new SessionTaskEntity("session", "title", "description", DateTime.MinValue));
        _sessionRepository.ValidateUserAccess("userId", "session").Returns(false);

        // Act
        var result = await _sut.Handle(new GetSuggestedEstimationRequest(1, new List<decimal>()));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Exception.Should().BeOfType<UserNotAuthorizedException>();
    }

    [Test]
    public async Task Handle_WhenPokerSuggestionServiceReturnsNull_ReturnsSuggestionServiceErrorException()
    {
        // Arrange
        _userAccessor.UserId.Returns("userId");
        _sessionTaskRepository.GetTaskById(1).Returns(new SessionTaskEntity("session", "title", "description", DateTime.MinValue));
        _sessionRepository.ValidateUserAccess("userId", "session").Returns(true);
        _pokerSuggestionService.GetSuggestedEstimation("title", "description", Arg.Any<List<decimal>>()).Returns((GetSuggestedEstimationResult?)null);

        // Act
        var result = await _sut.Handle(new GetSuggestedEstimationRequest(1, new List<decimal>()));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Exception.Should().BeOfType<SuggestionServiceErrorException>();
    }

    [Test]
    public async Task Handle_WhenAllConditionsAreMet_ReturnsGetSuggestedEstimationResponse()
    {
        // Arrange
        _userAccessor.UserId.Returns("userId");
        _sessionTaskRepository.GetTaskById(1).Returns(new SessionTaskEntity("session", "title", "description", DateTime.MinValue));
        _sessionRepository.ValidateUserAccess("userId", "session").Returns(true);
        _pokerSuggestionService.GetSuggestedEstimation("title", "description", Arg.Any<List<decimal>>()).Returns(new GetSuggestedEstimationResult(1, "reason"));
        var expectedResult = new GetSuggestedEstimationResponse(1, "reason");

        // Act
        var result = await _sut.Handle(new GetSuggestedEstimationRequest(1, new List<decimal>()));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResult);
    }
}
