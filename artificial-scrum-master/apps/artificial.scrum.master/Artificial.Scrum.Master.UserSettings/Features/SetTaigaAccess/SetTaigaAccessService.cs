using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.UserSettings.Features.SetTaigaAccess;

internal interface ISetTaigaAccessService
{
    Task Handle(string userId, TaigaAccess taigaAccess);
}

internal class SetTaigaAccessService : ISetTaigaAccessService
{
    private readonly IUserSettingsRepository _userSettingsRepository;

    public SetTaigaAccessService(IUserSettingsRepository userSettingsRepository)
    {
        _userSettingsRepository = userSettingsRepository;
    }

    public async Task Handle(string userId, TaigaAccess taigaAccess)
    {
        var taigaAccessString = JsonSerializer.Serialize(taigaAccess);
        var entity = await _userSettingsRepository.GetUserSettings(userId);

        if (entity.HasValue)
        {
            await _userSettingsRepository.UpdateUserSettings(entity.Value with
            {
                TaigaAccess = taigaAccessString
            });
        }
        else
        {
            await _userSettingsRepository.AddUserSettings(new UserSettingsEntity
            {
                TaigaAccess = taigaAccessString,
                UserId = userId
            });
        }
    }
}
