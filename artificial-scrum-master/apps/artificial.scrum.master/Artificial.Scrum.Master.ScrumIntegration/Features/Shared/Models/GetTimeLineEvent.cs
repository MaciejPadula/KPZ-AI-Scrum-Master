using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.ResponseEnums;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;

internal readonly record struct GetTimeLineEvent(
    int EventId,
    ScrumObjectType ScrumObjectType,
    ScrumObjectState ScrumObjectState,
    DateTime Created,
    int ProjectId,
    string ProjectName,
    int ObjectId,
    string ObjectName,
    int UserId,
    string UserName,
    string? UserPhoto,
    string? UserNick,
    List<KeyValuePair<string, string>> ValuesDiff
);
