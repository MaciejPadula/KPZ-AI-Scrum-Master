using Artificial.Scrum.Master.UserSettings.Features.SetTaigaAccess;
using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;
using NSubstitute;

namespace Artificial.Scrum.Master.UserSettings.Tests;

public class SetTaigaAccessServiceTests
{
    private SetTaigaAccessService _sut;
    private IUserSettingsRepository _userSettingsRepository;

    [SetUp]
    public void SetUp()
    {
        _userSettingsRepository = Substitute.For<IUserSettingsRepository>();
        _sut = new SetTaigaAccessService(_userSettingsRepository);
    }

    [Test]
    public async Task Handle_WhenSettingsAlreadyExists_ShouldCallUpdate()
    {
        // Arrange
        var userId = "userid";
        var taigaAccess = new TaigaAccess("some_key", "refresh");
        var expectedEntityCall = new UserSettingsEntity("userid", "some_key", "refresh");
        _userSettingsRepository.GetUserSettings(userId).Returns(new UserSettingsEntity("userid", "some_key", "refresh"));

        // Act
        await _sut.Handle(userId, taigaAccess);

        // Assert
        await _userSettingsRepository.Received(1).UpdateUserSettings(Arg.Is(expectedEntityCall));
        await _userSettingsRepository.Received(0).AddUserSettings(Arg.Any<UserSettingsEntity>());
    }

    [Test]
    public async Task Handle_WhenSettingsDoesNotExists_ShouldCallAdd()
    {
        // Arrange
        var userId = "userid";
        var taigaAccess = new TaigaAccess("some_key", "refresh");
        var expectedEntityCall = new UserSettingsEntity("userid", "some_key", "refresh");
        _userSettingsRepository.GetUserSettings(userId).Returns((UserSettingsEntity?)null);

        // Act
        await _sut.Handle(userId, taigaAccess);

        // Assert
        await _userSettingsRepository.Received(0).UpdateUserSettings(Arg.Any<UserSettingsEntity>());
        await _userSettingsRepository.Received(1).AddUserSettings(Arg.Is(expectedEntityCall));
    }
}
