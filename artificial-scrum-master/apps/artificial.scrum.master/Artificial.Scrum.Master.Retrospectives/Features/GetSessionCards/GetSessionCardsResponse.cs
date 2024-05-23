namespace Artificial.Scrum.Master.Retrospectives.Features.GetSessionCards;

internal record GetSessionCardsResponse(
    List<string> Goods,
    List<string> Bads,
    List<string> Ideas);
