namespace Artificial.Scrum.Master.UserSettings.Infrastructure.Models;

public record UserSettingsEntity(
    string UserId,
    string TaigaAccessToken,
    string TaigaRefreshToken);
