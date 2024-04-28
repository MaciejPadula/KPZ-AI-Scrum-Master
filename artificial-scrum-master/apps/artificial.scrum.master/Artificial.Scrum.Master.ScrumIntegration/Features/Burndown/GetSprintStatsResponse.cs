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
    List<KeyValuePair<string, double>> TotalRolePoints,
    double CompletedRolePoints
);

internal readonly record struct SprintDayStats(
    string Day,
    int Name,
    double OpenPoints,
    double OptimalPoints
);
