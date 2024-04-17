using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation;
using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.EstimationPoker.Tests;

public class AddTaskEstimationServiceTests
{
    private AddTaskEstimationService _sut;
    private IRequestValidator _requestValidator;
    private ISessionTaskRepository _sessionTaskRepository;

    [SetUp]
    public void SetUp()
    {
        _requestValidator = Substitute.For<IRequestValidator>();
        _sessionTaskRepository = Substitute.For<ISessionTaskRepository>();
        _sut = new AddTaskEstimationService(_requestValidator, _sessionTaskRepository);
    }

    [Test]
    public async Task Handle_WhenRequestIsInvalid_ReturnsError()
    {
        // Arrange
        var request = new AddTaskEstimationRequest(
            "",
            "",
            2,
            2137);
        _requestValidator.Validate(request).Returns(new ArgumentNullException());

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Exception.Should().BeEquivalentTo(new ArgumentNullException());
        await _sessionTaskRepository.Received(0).AddTaskEstimation(Arg.Any<SessionTaskEstimationEntity>());
    }

    [Test]
    public async Task Handle_WhenRequestIsValid_ReturnsSuccess()
    {
        // Arrange
        var request = new AddTaskEstimationRequest(
            "session-id",
            "maciej",
            2,
            2137);
        _requestValidator.Validate(request).Returns((Exception?)null);

        // Act
        var result = await _sut.Handle(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _sessionTaskRepository.Received(1).AddTaskEstimation(Arg.Is<SessionTaskEstimationEntity>(x =>
            x.TaskId == 2
            && x.Username == "maciej"
            && x.Value == 2137));
    }
}
