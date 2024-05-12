using Microsoft.AspNetCore.Authorization;

namespace Artificial.Scrum.Master.Infrastructure.Authorization.Requirements;

internal class LoggedInRequirement : IAuthorizationRequirement
{
    public List<string> RequiredClaimTypes { get; }

    public LoggedInRequirement(List<string> requiredClaimTypes)
    {
        RequiredClaimTypes = requiredClaimTypes;
    }
}
