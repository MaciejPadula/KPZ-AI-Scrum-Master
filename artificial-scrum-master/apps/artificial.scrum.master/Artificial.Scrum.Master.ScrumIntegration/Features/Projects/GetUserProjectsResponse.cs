namespace Artificial.Scrum.Master.ScrumIntegration.Features.Projects;

public readonly record struct GetUserProjectsResponse(
    int Id,
    string Name,
    DateTime ModifiedDate,
    bool IsPrivate,
    bool AmOwner,
    string OwnerUsername
);
