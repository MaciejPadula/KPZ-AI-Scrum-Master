namespace Artificial.Scrum.Master.ScrumIntegration.Features.Projects;

internal readonly record struct GetUserProjectsResponse(
    List<GetUserProjectsResponseElement> Projects
);

internal readonly record struct GetUserProjectsResponseElement(
    int Id,
    string Name,
    DateTime ModifiedDate,
    bool IsPrivate,
    bool AmOwner,
    string OwnerUsername
);
