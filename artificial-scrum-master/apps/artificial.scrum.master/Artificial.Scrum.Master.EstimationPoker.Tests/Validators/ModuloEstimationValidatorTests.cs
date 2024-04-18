using Artificial.Scrum.Master.EstimationPoker.Features.AddTaskEstimation.Validator.Estimation;
using FluentAssertions;

namespace Artificial.Scrum.Master.EstimationPoker.Tests.Validators;

public class ModuloEstimationValidatorTests
{
    private ModuloEstimationValidator _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new ModuloEstimationValidator();
    }

    [Test]
    [TestCase(12.5, true)]
    [TestCase(11.25, false)]
    [TestCase(13.75, false)]
    [TestCase(0.5, true)]
    [TestCase(0.33, false)]
    [TestCase(12, true)]
    public void Validate_WhenValueIsProvided_ShouldReturnExpectedResult(decimal value, bool expectedResult)
    {
        // Act
        var result = _sut.ValidateEstimationValue(value);

        // Assert
        result.Should().Be(expectedResult);
    }
}
