using Artificial.Scrum.Master.Retrospectives.Features.CreateSessionCard;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.Retrospectives.Tests;

public class CreateSessionCardHandlerTests
{
    private CreateSessionCardHandler _sut;
    private IRetroSessionRepository _repository;
    private TimeProvider _timeProvider;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<IRetroSessionRepository>();
        _timeProvider = Substitute.For<TimeProvider>();
        _sut = new CreateSessionCardHandler(_repository, _timeProvider);
    }

    [Test]
    public async Task Handle_ShouldCreateSessionCard()
    {
        // Arrange
        var request = new CreateSessionCardRequest("content", CardType.Good, "sessionId");

        // Act
        await _sut.Handle(request);

        // Assert
        await _repository.Received(1).CreateSessionCard(Arg.Is<SessionCard>(x =>
            x.Content == request.Content &&
            x.Type == request.Type &&
            x.SessionId == request.SessionId));
    }

    [Test]
    public async Task Handle_WhenCardTypeIsInvalid_ShouldReturnError()
    {
        // Arrange
        var request = new CreateSessionCardRequest("content", (CardType)int.MaxValue, "sessionId");

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<InvalidCardTypeException>();
    }
}
