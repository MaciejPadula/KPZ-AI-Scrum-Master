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
        if (!result.HasValue)
        {
            return new();
        }

        var taigaAccess = JsonSerializer.Deserialize<TaigaAccess>(result.Value.TaigaAccess);
        return new Settings(taigaAccess);
    }
}
