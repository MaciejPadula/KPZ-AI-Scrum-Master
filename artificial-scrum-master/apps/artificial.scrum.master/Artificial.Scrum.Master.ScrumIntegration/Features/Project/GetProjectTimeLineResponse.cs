namespace Artificial.Scrum.Master.ScrumIntegration.Features.Project;

public readonly record struct GetProjectTimeLineResponse(
    int Id,
    string? EventType,
    DateTime Created,
    int ProjectId,
    string? Comment,
    string? CommentHtml,
    // Data
    int TaskId,
    string? TaskSubject,
    int UserStoryId,
    string? UserStorySubject,
    // User
    int UserId,
    string UserName,
    string? UserPhoto,
    string? UserUsername,
    // Project
    string ProjectName,
    // ValuesDiff
    ValuesDiff? ValuesDiff
);
