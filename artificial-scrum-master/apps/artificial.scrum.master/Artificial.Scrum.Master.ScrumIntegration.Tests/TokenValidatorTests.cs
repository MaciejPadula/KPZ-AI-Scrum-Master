using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.ScrumIntegration.Tests;

internal class TokenValidatorTests
{
    private TokenValidator _sut;
    private TimeProvider _timeProvider;
    private IJwtDecoder _jwtDecoder;

    [SetUp]
    public void SetUp()
    {
        _timeProvider = Substitute.For<TimeProvider>();
        _jwtDecoder = Substitute.For<IJwtDecoder>();
        _sut = new TokenValidator(
            _timeProvider,
            _jwtDecoder);
    }

    [Test]
    public void ValidateAccessTokenExpirationTime_WhenTokenIsExpired_ShouldReturnFalse()
    {
        // Arrange
        var expirationDate = new DateTime(2021, 11, 1, 0, 0, 0, DateTimeKind.Utc);
        var now = new DateTime(2021, 11, 1, 12, 0, 0, DateTimeKind.Utc);
        _timeProvider.GetUtcNow().Returns(now);
        _jwtDecoder.GetExpirationDate(Arg.Any<string>(), Arg.Any<string>()).Returns(expirationDate);

        // Act
        var result = _sut.ValidateAccessTokenExpirationTime("token");

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ValidateAccessTokenExpirationTime_WhenTokenIsNotFound_ShouldReturnFalse()
    {
        // Arrange
        var now = new DateTime(2021, 11, 1, 0, 0, 1, DateTimeKind.Utc);
        _timeProvider.GetUtcNow().Returns(now);
        _jwtDecoder.GetExpirationDate(Arg.Any<string>(), Arg.Any<string>()).Returns((DateTime?)null);

        // Act
        var result = _sut.ValidateAccessTokenExpirationTime("token");

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ValidateAccessTokenExpirationTime_WhenTokenIsNotExpired_ShouldReturnTrue()
    {
        // Arrange
        var expirationDate = new DateTime(2021, 11, 1, 0, 0, 2, DateTimeKind.Utc);
        var now = new DateTime(2021, 11, 1, 0, 0, 1, DateTimeKind.Utc);
        _timeProvider.GetUtcNow().Returns(now);
        _jwtDecoder.GetExpirationDate(Arg.Any<string>(), Arg.Any<string>()).Returns(expirationDate);

        // Act
        var result = _sut.ValidateAccessTokenExpirationTime("token");

        // Assert
        result.Should().BeTrue();
    }
}
