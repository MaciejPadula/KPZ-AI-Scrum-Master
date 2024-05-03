namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;

public record UserTokens(
    string UserId,
    string AccessToken,
    string RefreshToken
);
