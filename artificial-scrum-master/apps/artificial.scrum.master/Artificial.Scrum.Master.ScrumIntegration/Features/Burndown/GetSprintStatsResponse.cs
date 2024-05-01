namespace Artificial.Scrum.Master.ScrumIntegration.Features.Burndown;

internal readonly record struct GetSprintStatsResponse(
    string SprintName,
    string EstimatedStart,
    string EstimatedFinish,
    int TotalUserStories,
    int CompletedUserStories,
    int TotalTasks,
    int CompletedTasks,
    List<SprintDayStats> SprintDayStats,
    List<KeyValuePair<string, decimal>> TotalRolePoints,
    decimal CompletedRolePoints
);

internal readonly record struct SprintDayStats(
    string Day,
    decimal OpenPoints,
    decimal OptimalPoints
);
