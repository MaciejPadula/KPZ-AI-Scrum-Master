using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;

namespace Artificial.Scrum.Master.Retrospectives.Features.GetSuggestionForCard;

internal record GetSuggestionForCardRequest(
    string CardContent,
    CardType CardType);
