namespace Artificial.Scrum.Master.ScrumProjectIntegration.Utilities;

public interface IJwtDecoder
{
    public string? GetClaim(string token, string claimTypeName);
    public DateTime GetExpirationDate(string token, string claimTypeName);
}
