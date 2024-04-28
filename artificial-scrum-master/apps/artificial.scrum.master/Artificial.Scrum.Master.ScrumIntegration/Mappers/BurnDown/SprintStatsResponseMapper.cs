using Artificial.Scrum.Master.ScrumIntegration.Features.Burndown;
using System.Text.RegularExpressions;

namespace Artificial.Scrum.Master.ScrumIntegration.Mappers.BurnDown;

internal interface ISprintStatsResponseMapper
{
    GetSprintStatsResponse MapSprintStatsResponse(SprintStats sprintStats, SprintData sprintData);
}

internal class SprintStatsResponseMapper : ISprintStatsResponseMapper
{
    private static readonly Regex RoleRegex = new(@"\D");

    public GetSprintStatsResponse MapSprintStatsResponse(SprintStats sprintStats, SprintData sprintData)
    {
        var sprintDayStats = sprintStats.Days?.Select(day => new SprintDayStats(
            Day: day.Day ?? string.Empty,
            Name: day.Name,
            OpenPoints: day.OpenPoints,
            OptimalPoints: day.OptimalPoints
        )).ToList();

        var totalRolePoints = MapTotalRolePoints(sprintStats.TotalPoints, sprintData);
        var completedPoints = sprintStats.CompletedPoints?.Sum() ?? 0;

        return new GetSprintStatsResponse(
            SprintName: sprintStats.Name ?? string.Empty,
            EstimatedStart: sprintStats.EstimatedStart ?? string.Empty,
            EstimatedFinish: sprintStats.EstimatedFinish ?? string.Empty,
            TotalUserStories: sprintStats.TotalUserStories,
            CompletedUserStories: sprintStats.CompletedUserStories,
            TotalTasks: sprintStats.TotalTasks,
            CompletedTasks: sprintStats.CompletedTasks,
            SprintDayStats: sprintDayStats ?? [],
            TotalRolePoints: totalRolePoints,
            CompletedRolePoints: completedPoints
        );
    }

    private static List<KeyValuePair<string, double>> MapTotalRolePoints(
        TotalPoints? totalPoints,
        SprintData sprintData)
    {
        List<KeyValuePair<string, double>> rolePoints = [];
        if (totalPoints is null)
        {
            return rolePoints;
        }

        var roleIds = new Dictionary<string, double>
        {
            { nameof(TotalPoints.RoleId9262869), totalPoints.RoleId9262869 },
            { nameof(TotalPoints.RoleId9262870), totalPoints.RoleId9262870 },
            { nameof(TotalPoints.RoleId9262871), totalPoints.RoleId9262871 },
            { nameof(TotalPoints.RoleId9262872), totalPoints.RoleId9262872 }
        };

        foreach (var roleId in roleIds)
        {
            var extractedId = RoleRegex.Replace(roleId.Key, "");
            var success = int.TryParse(extractedId, out var id);
            if (!success)
            {
                continue;
            }

            var roleMatch = sprintData.Roles?.FirstOrDefault(role => role.Id == id);
            if (roleMatch is not null)
            {
                rolePoints.Add(new KeyValuePair<string, double>(roleMatch.Name ?? string.Empty, roleId.Value));
            }
        }

        return rolePoints;
    }
}
