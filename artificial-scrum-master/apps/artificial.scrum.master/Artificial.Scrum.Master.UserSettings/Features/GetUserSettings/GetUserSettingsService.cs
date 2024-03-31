using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;

namespace Artificial.Scrum.Master.UserSettings.Features.GetUserSettings;

internal interface IGetUserSettingsService
{
    Task<Settings> Handle(string userId);
}

internal class GetUserSettingsService : IGetUserSettingsService
{
    private readonly IUserSettingsRepository _userSettingsRepository;

    public GetUserSettingsService(IUserSettingsRepository userSettingsRepository)
    {
        _userSettingsRepository = userSettingsRepository;
    }

    public async Task<Settings> Handle(string userId)
    {
        var result = await _userSettingsRepository.GetUserSettings(userId);
        if (result is null)
        {
            return new();
        }

        return new Settings
        {
            IsLoggedToTaiga = ValidateTaigaAccess(result)
        };
    }

    private bool ValidateTaigaAccess(UserSettingsEntity? userSettings)
    {
        return userSettings is not null
            && !string.IsNullOrEmpty(userSettings.TaigaAccessToken)
            && !string.IsNullOrEmpty(userSettings.TaigaRefreshToken);
    }
}
