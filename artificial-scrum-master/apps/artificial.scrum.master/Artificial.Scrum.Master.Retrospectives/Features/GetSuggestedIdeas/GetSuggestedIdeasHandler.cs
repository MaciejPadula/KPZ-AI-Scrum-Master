using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Exceptions;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using Artificial.Scrum.Master.SharedKernel;

namespace Artificial.Scrum.Master.Retrospectives.Features.GetSuggestedIdeas;

internal interface IGetSuggestedIdeasHandler
{
    Task<Result<GetSuggestedIdeasResponse>> Handle(string sessionId);
}

internal class GetSuggestedIdeasHandler : IGetSuggestedIdeasHandler
{
    private readonly IUserAccessor _userAccessor;
    private readonly IRetroSessionRepository _retroSessionRepository;
    private readonly IRetroSuggestionService _retroSuggestionService;

    public GetSuggestedIdeasHandler(
        IUserAccessor userAccessor,
        IRetroSessionRepository retroSessionRepository,
        IRetroSuggestionService retroSuggestionService)
    {
        _userAccessor = userAccessor;
        _retroSessionRepository = retroSessionRepository;
        _retroSuggestionService = retroSuggestionService;
    }

    public async Task<Result<GetSuggestedIdeasResponse>> Handle(string sessionId)
    {
        var userId = _userAccessor.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return new UnauthorizedAccessException();
        }

        if (!await _retroSessionRepository.SessionExists(sessionId))
        {
            return new SessionNotFoundException(sessionId);
        }

        var cards = await _retroSessionRepository.GetSessionCards(sessionId);
        var suggestion = await _retroSuggestionService.GetSuggestedIdeas(cards);

        return new GetSuggestedIdeasResponse(suggestion?.Ideas ?? []);
    }
}
