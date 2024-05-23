using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;

namespace Artificial.Scrum.Master.Retrospectives.Features.CreateSessionCard;

public record CreateSessionCardRequest(
    string Content,
    CardType Type,
    string SessionId);
