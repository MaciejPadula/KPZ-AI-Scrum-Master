using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.UserSettings.Features.GetUserSettings;
using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;
using FluentAssertions;
using NSubstitute;

namespace Artificial.Scrum.Master.UserSettings.Tests;

public class GetUserSettingsServiceTests
{
    private GetUserSettingsService _sut;
    private IUserSettingsRepository _userSettingsRepository;
    private IUserAccessor _userAccessor;

    [SetUp]
    public void SetUp()
    {
        _userSettingsRepository = Substitute.For<IUserSettingsRepository>();
        _userAccessor = Substitute.For<IUserAccessor>();
        _sut = new GetUserSettingsService(_userSettingsRepository, _userAccessor);
    }

    [Test]
    public async Task Handle_WhenUserSettingsAreNull_ReturnsEmptySettings()
    {
        // Arrange
        _userAccessor.UserId.Returns("userid");
        _userSettingsRepository.GetUserSettings("userid").Returns((UserSettingsEntity?)null);
        var expected = new Settings();

        // Act
        var result = await _sut.Handle();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    [TestCase("AccessToken", "RefreshToken", true)]
    [TestCase("", "RefreshToken", false)]
    [TestCase("AccessToken", "", false)]
    [TestCase("", "", false)]
    public async Task Handle_WhenUserSettings_ShouldReturnCorrectTaigaFlag(
        string accessToken, string refreshToken, bool expectedIsLoggedToTaiga)
    {
        // Arrange
        _userAccessor.UserId.Returns("userid");
        var userSettingsEntity = new UserSettingsEntity("userid", accessToken, refreshToken);
        _userSettingsRepository.GetUserSettings("userid").Returns(userSettingsEntity);

        // Act
        var result = await _sut.Handle();

        // Assert
        result.IsLoggedToTaiga.Should().Be(expectedIsLoggedToTaiga);
    }
}
