namespace Artificial.Scrum.Master.UserSettings.Infrastructure.Models;

public readonly record struct UserSettingsEntity(
    string UserId,
    string TaigaApiKey);
