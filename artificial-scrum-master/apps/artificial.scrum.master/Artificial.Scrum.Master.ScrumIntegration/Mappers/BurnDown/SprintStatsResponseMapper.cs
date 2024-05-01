using Artificial.Scrum.Master.ScrumIntegration.Features.Burndown;

namespace Artificial.Scrum.Master.ScrumIntegration.Mappers.BurnDown;

internal interface ISprintStatsResponseMapper
{
    GetSprintStatsResponse MapSprintStatsResponse(SprintStats sprintStats, SprintData sprintData);
}

internal class SprintStatsResponseMapper : ISprintStatsResponseMapper
{
    private readonly IRolePointsMapper _rolePointsMapper;

    public SprintStatsResponseMapper(IRolePointsMapper rolePointsMapper)
    {
        _rolePointsMapper = rolePointsMapper;
    }

    public GetSprintStatsResponse MapSprintStatsResponse(SprintStats sprintStats, SprintData sprintData)
    {
        var sprintDayStats = sprintStats.Days?.Select(day => new SprintDayStats(
            Day: day.Day ?? string.Empty,
            Name: day.Name,
            OpenPoints: day.OpenPoints,
            OptimalPoints: day.OptimalPoints
        )).ToList();

        var totalRolePoints = _rolePointsMapper.MapTotalRolePoints(sprintStats.TotalPoints, sprintData);
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
}
