using Artificial.Scrum.Master.Interfaces;
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
    private IUserAccessor _userAccessor;

    [SetUp]
    public void SetUp()
    {
        _userSettingsRepository = Substitute.For<IUserSettingsRepository>();
        _userAccessor = Substitute.For<IUserAccessor>();
        _sut = new SetTaigaAccessService(_userSettingsRepository, _userAccessor);
    }

    [Test]
    public async Task Handle_WhenSettingsAlreadyExists_ShouldCallUpdate()
    {
        // Arrange
        _userAccessor.UserId.Returns("userid");
        var request = new SetTaigaAccessRequest("some_key", "refresh");
        var expectedEntityCall = new UserSettingsEntity("userid", "some_key", "refresh");
        _userSettingsRepository.GetUserSettings("userid").Returns(new UserSettingsEntity("userid", "some_key", "refresh"));

        // Act
        await _sut.Handle(request);

        // Assert
        await _userSettingsRepository.Received(1).UpdateUserSettings(Arg.Is(expectedEntityCall));
        await _userSettingsRepository.Received(0).AddUserSettings(Arg.Any<UserSettingsEntity>());
    }

    [Test]
    public async Task Handle_WhenSettingsDoesNotExists_ShouldCallAdd()
    {
        // Arrange
        _userAccessor.UserId.Returns("userid");
        var request = new SetTaigaAccessRequest("some_key", "refresh");
        var expectedEntityCall = new UserSettingsEntity("userid", "some_key", "refresh");
        _userSettingsRepository.GetUserSettings("userid").Returns((UserSettingsEntity?)null);

        // Act
        await _sut.Handle(request);

        // Assert
        await _userSettingsRepository.Received(0).UpdateUserSettings(Arg.Any<UserSettingsEntity>());
        await _userSettingsRepository.Received(1).AddUserSettings(Arg.Is(expectedEntityCall));
    }
}
