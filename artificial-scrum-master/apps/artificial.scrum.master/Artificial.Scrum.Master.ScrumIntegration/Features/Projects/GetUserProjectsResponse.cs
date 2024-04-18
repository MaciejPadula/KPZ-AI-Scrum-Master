namespace Artificial.Scrum.Master.ScrumIntegration.Features.Projects;

internal readonly record struct GetUserProjectsResponse(
    List<UserProject> Projects
);

internal readonly record struct UserProject(
    int Id,
    string Name,
    DateTime ModifiedDate,
    bool IsPrivate,
    bool AmOwner,
    string OwnerUsername
);
