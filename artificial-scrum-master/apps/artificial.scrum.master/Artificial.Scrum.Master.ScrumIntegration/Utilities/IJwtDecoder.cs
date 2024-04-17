namespace Artificial.Scrum.Master.ScrumIntegration.Utilities;

internal interface IJwtDecoder
{
    public string? GetClaim(string token, string claimTypeName);
    public DateTime? GetExpirationDate(string token, string claimTypeName);
}
