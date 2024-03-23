using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;

namespace Artificial.Scrum.Master.UserSettings.Features.SetUserSettings;

internal interface ISetUserSettingsService
{
    Task Handle(string userId, Settings settings);
}

internal class SetUserSettingsService : ISetUserSettingsService
{
    private readonly IUserSettingsRepository _userSettingsRepository;

    public SetUserSettingsService(IUserSettingsRepository userSettingsRepository)
    {
        _userSettingsRepository = userSettingsRepository;
    }

    public async Task Handle(string userId, Settings settings)
    {
        var alreadyExists = await _userSettingsRepository.UserSettingsExists(userId);
        var entity = new UserSettingsEntity(userId, settings.TaigaApiKey);

        if (alreadyExists)
        {
            await _userSettingsRepository.UpdateUserSettings(entity);
        }
        else
        {
            await _userSettingsRepository.AddUserSettings(entity);
        }
    }
}
