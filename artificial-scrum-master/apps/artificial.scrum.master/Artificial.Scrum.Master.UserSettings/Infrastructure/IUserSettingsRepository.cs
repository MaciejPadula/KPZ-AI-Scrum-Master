using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;

namespace Artificial.Scrum.Master.UserSettings.Infrastructure;

public interface IUserSettingsRepository
{
    Task<UserSettingsEntity> GetUserSettings(string userId);
    Task UpdateUserSettings(UserSettingsEntity userSettings);
    Task AddUserSettings(UserSettingsEntity userSettings);
    Task<bool> UserSettingsExists(string userId);
}
