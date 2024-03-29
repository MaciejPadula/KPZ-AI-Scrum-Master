namespace Artificial.Scrum.Master.UserSettings.Features.SetTaigaAccess;

public readonly record struct SetTaigaAccessRequest(
    string AccessToken,
    string RefreshToken);
