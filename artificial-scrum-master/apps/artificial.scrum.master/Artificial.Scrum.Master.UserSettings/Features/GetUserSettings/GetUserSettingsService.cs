using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        return new Settings(result.TaigaApiKey);
    }
}
