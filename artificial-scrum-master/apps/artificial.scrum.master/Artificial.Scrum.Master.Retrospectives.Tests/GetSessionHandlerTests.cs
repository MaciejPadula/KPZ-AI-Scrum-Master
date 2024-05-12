using Artificial.Scrum.Master.Retrospectives.Features.GetSprintSession;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.Retrospectives.Tests;

public class GetSessionHandlerTests
{
    private GetSessionHandler _sut;
    private IRetroSessionRepository _retroSessionRepository;

    [SetUp]
    public void Setup()
    {
        _retroSessionRepository = Substitute.For<IRetroSessionRepository>();
        _sut = new GetSessionHandler(_retroSessionRepository);
    }

    [Test]
    public async Task Handle_WhenSessionDoesNotExist_ReturnsSessionNotFoundException()
    {
        var sessionId = Guid.NewGuid().ToString();
        _retroSessionRepository.GetSession(sessionId).Returns((RetroSession?)null);

        var result = await _sut.Handle(sessionId);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Exception.Should().BeOfType<SessionNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenSessionExists_ReturnsValidResult()
    {
        var sessionId = Guid.NewGuid().ToString();
        var session = new RetroSession(sessionId, "name", 1, 2);
        _retroSessionRepository.GetSession(sessionId).Returns(session);

        var result = await _sut.Handle(sessionId);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be("name");
    }
}
