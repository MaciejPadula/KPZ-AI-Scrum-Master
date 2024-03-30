using Artificial.Scrum.Master.UserSettings.Features.GetUserSettings;
using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;
using FluentAssertions;
using NSubstitute;
using System.Text.Json;

namespace Artificial.Scrum.Master.UserSettings.Tests;

public class GetUserSettingsServiceTests
{
    private GetUserSettingsService _sut;
    private IUserSettingsRepository _userSettingsRepository;

    [SetUp]
    public void SetUp()
    {
        _userSettingsRepository = Substitute.For<IUserSettingsRepository>();
        _sut = new GetUserSettingsService(_userSettingsRepository);
    }

    [Test]
    public async Task Handle_WhenUserSettingsAreNull_ReturnsEmptySettings()
    {
        // Arrange
        string userId = "1";
        _userSettingsRepository.GetUserSettings(userId).Returns((UserSettingsEntity?)null);
        var expected = new Settings();

        // Act
        var result = await _sut.Handle(userId);

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
        string userId = "1";
        var userSettingsEntity = new UserSettingsEntity(userId, accessToken, refreshToken);
        _userSettingsRepository.GetUserSettings(userId).Returns(userSettingsEntity);

        // Act
        var result = await _sut.Handle(userId);

        // Assert
        result.IsLoggedToTaiga.Should().Be(expectedIsLoggedToTaiga);
    }
}
