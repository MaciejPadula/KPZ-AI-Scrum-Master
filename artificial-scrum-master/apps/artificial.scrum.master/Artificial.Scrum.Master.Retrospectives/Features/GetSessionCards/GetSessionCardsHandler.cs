using Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using Artificial.Scrum.Master.SharedKernel;

namespace Artificial.Scrum.Master.Retrospectives.Features.GetSessionCards;

internal interface IGetSessionCardsHandler
{
    Task<Result<GetSessionCardsResponse>> Handle(string sessionId);
}

internal class GetSessionCardsHandler : IGetSessionCardsHandler
{
    private readonly IRetroSessionRepository _retroSessionRepository;

    public GetSessionCardsHandler(IRetroSessionRepository retroSessionRepository)
    {
        _retroSessionRepository = retroSessionRepository;
    }

    public async Task<Result<GetSessionCardsResponse>> Handle(string sessionId)
    {
        if (await _retroSessionRepository.SessionExists(sessionId) == false)
        {
            return new SessionNotFoundException(sessionId);
        }

        var cards = await _retroSessionRepository.GetSessionCards(sessionId);

        var groupedCards = cards
            .GroupBy(x => x.Type)
            .ToDictionary(
                cardType => cardType.Key,
                cardType => cardType
                    .Select(card => card.Content)
                    .ToList());

        return new GetSessionCardsResponse(
            groupedCards.GetValueOrDefault(CardType.Good, []),
            groupedCards.GetValueOrDefault(CardType.Bad, []),
            groupedCards.GetValueOrDefault(CardType.Ideas, []));
    }
}
