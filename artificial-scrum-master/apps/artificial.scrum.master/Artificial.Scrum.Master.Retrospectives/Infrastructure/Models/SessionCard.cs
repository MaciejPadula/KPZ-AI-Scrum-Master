using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;

namespace Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;

public readonly record struct SessionCard(
    string Content,
    CardType Type,
    string SessionId,
    DateTime CreatedAt);
