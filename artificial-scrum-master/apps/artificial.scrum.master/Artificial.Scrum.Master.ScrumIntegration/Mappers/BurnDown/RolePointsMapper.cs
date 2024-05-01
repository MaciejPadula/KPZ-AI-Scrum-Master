using Artificial.Scrum.Master.ScrumIntegration.Features.Burndown;
using System.Text.RegularExpressions;

namespace Artificial.Scrum.Master.ScrumIntegration.Mappers.BurnDown;

internal interface IRolePointsMapper
{
    List<KeyValuePair<string, decimal>> MapTotalRolePoints(TotalPoints? totalPoints, SprintData sprintData);
}

internal partial class RolePointsMapper : IRolePointsMapper
{
    [GeneratedRegex(@"\D")]
    private static partial Regex ExtractNumericValue();

    private readonly Regex _roleRegex = ExtractNumericValue();

    public List<KeyValuePair<string, decimal>> MapTotalRolePoints(TotalPoints? totalPoints, SprintData sprintData)
    {
        List<KeyValuePair<string, decimal>> rolePoints = [];
        if (totalPoints is null)
        {
            return rolePoints;
        }

        var roleIds = new Dictionary<string, decimal>
        {
            { nameof(TotalPoints.RoleId9262869), totalPoints.RoleId9262869 },
            { nameof(TotalPoints.RoleId9262870), totalPoints.RoleId9262870 },
            { nameof(TotalPoints.RoleId9262871), totalPoints.RoleId9262871 },
            { nameof(TotalPoints.RoleId9262872), totalPoints.RoleId9262872 }
        };

        foreach (var roleId in roleIds)
        {
            var extractedId = _roleRegex.Replace(roleId.Key, "");
            var success = int.TryParse(extractedId, out var id);
            if (!success)
            {
                continue;
            }

            var roleMatch = sprintData.Roles?.FirstOrDefault(role => role.Id == id);
            if (roleMatch is not null)
            {
                rolePoints.Add(new KeyValuePair<string, decimal>(roleMatch.Name ?? string.Empty, roleId.Value));
            }
        }

        return rolePoints;
    }
}
