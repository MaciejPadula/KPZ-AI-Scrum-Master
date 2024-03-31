namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

public readonly record struct GetProfileTimeLineResponse(
    int Id,
    string? EventType,
    DateTime Created,
    int ProjectId,
    string? Comment,
    string? CommentHtml,
    // Data
    int TaskId,
    string? TaskSubject,
    int TaskUserStoryId,
    string? TaskUserStorySubject,
    // User
    int UserId,
    string UserName,
    string? UserPhoto,
    string? UserUsername,
    // Project
    string ProjectName,
    // ValuesDiff
    ValuesDiff? ValuesDiff,
    // Milestone
    Milestone? Milestone,
    // Userstory
    Userstory? Userstory
);
