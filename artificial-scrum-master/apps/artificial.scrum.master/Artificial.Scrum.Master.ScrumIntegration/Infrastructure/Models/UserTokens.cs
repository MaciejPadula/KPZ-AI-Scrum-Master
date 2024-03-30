namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;

public readonly record struct UserTokens(
    string AccessToken,
    string RefreshToken
);
