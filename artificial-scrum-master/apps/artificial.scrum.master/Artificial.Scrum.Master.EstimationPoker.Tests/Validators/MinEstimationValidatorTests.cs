using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator.Estimation;
using FluentAssertions;

namespace Artificial.Scrum.Master.EstimationPoker.Tests.Validators;

public class MinEstimationValidatorTests
{
    private MinEstimationValidator _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new MinEstimationValidator();
    }

    [Test]
    [TestCase(2137, true)]
    [TestCase(-420, false)]
    [TestCase(21.37, true)]
    [TestCase(1, true)]
    [TestCase(0, false)]
    [TestCase(-1, false)]
    public void ValidateEstimationValue_WhenValueIsProvided_ShouldReturnExpectedResult(decimal value, bool expectedResult)
    {
        // Act
        var result = _sut.ValidateEstimationValue(value);

        // Assert
        result.Should().Be(expectedResult);
    }
}
