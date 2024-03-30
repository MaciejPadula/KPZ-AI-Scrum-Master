namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models;

public readonly record struct UserTokens(
    string AccessToken,
    string RefreshToken
);
