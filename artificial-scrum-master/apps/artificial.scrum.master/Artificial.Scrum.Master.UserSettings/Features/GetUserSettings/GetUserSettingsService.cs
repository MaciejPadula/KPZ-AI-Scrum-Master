using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using System.Text.Json;

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

        var taigaAccess = DeserializeTaigaAccess(result.TaigaAccess);

        return new Settings
        {
            IsLoggedToTaiga = ValidateTaigaAccess(taigaAccess)
        };
    }

    private bool ValidateTaigaAccess(TaigaAccess? taigaAccess)
    {
        return taigaAccess.HasValue
            && !string.IsNullOrEmpty(taigaAccess.Value.AccessToken)
            && !string.IsNullOrEmpty(taigaAccess.Value.RefreshToken);
    }

    private TaigaAccess? DeserializeTaigaAccess(string taigaAccessString)
    {
        if (string.IsNullOrEmpty(taigaAccessString))
        {
            return null;
        }

        return JsonSerializer.Deserialize<TaigaAccess>(taigaAccessString);
    }
}
