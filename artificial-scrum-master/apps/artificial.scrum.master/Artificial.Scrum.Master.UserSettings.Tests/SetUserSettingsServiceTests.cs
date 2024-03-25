using Artificial.Scrum.Master.UserSettings.Features.SetUserSettings;
using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;
using NSubstitute;

namespace Artificial.Scrum.Master.UserSettings.Tests;

public class SetUserSettingsServiceTests
{
    private SetUserSettingsService _sut;
    private IUserSettingsRepository _userSettingsRepository;

    [SetUp]
    public void SetUp()
    {
        _userSettingsRepository = Substitute.For<IUserSettingsRepository>();
        _sut = new SetUserSettingsService(_userSettingsRepository);
    }

    [Test]
    public async Task Handle_WhenSettingsAlreadyExists_ShouldCallUpdate()
    {
        // Arrange
        var userId = "userid";
        var settings = new Settings(
            new TaigaAccess("some_key", "refresh"));
        var expectedEntityCall = new UserSettingsEntity("userid", "some_key");
        _userSettingsRepository.UserSettingsExists(userId).Returns(true);

        // Act
        await _sut.Handle(userId, settings);

        // Assert
        await _userSettingsRepository.Received(1).UpdateUserSettings(Arg.Is(expectedEntityCall));
    }

    [Test]
    public async Task Handle_WhenSettingsDoNotExists_ShouldCallCreate()
    {
        // Arrange
        var userId = "userid";
        var settings = new Settings(
            new TaigaAccess("some_key", "refresh"));
        var expectedEntityCall = new UserSettingsEntity("userid", "some_key");
        _userSettingsRepository.UserSettingsExists(userId).Returns(false);

        // Act
        await _sut.Handle(userId, settings);

        // Assert
        await _userSettingsRepository.Received(1).AddUserSettings(Arg.Is(expectedEntityCall));
    }
}
