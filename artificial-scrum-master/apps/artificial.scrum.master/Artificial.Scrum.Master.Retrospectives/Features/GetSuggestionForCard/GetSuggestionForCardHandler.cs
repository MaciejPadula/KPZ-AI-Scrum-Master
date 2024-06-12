using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using Artificial.Scrum.Master.SharedKernel;

namespace Artificial.Scrum.Master.Retrospectives.Features.GetSuggestionForCard;

internal interface IGetSuggestionForCardHandler
{
    Task<Result<GetSuggestionForCardResponse>> Handle(GetSuggestionForCardRequest request);
}

internal class GetSuggestionForCardHandler : IGetSuggestionForCardHandler
{
    private readonly IUserAccessor _userAccessor;
    private readonly IRetroSuggestionService _retroSuggestionService;

    public GetSuggestionForCardHandler(
        IUserAccessor userAccessor,
        IRetroSuggestionService retroSuggestionService)
    {
        _userAccessor = userAccessor;
        _retroSuggestionService = retroSuggestionService;
    }

    public async Task<Result<GetSuggestionForCardResponse>> Handle(GetSuggestionForCardRequest request)
    {
        var userId = _userAccessor.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return new UnauthorizedAccessException();
        }

        var suggestion = await _retroSuggestionService.GetSuggestedIdeas([
            new SessionCard
            {
                Type = request.CardType,
                Content = request.CardContent
            }]);

        return new GetSuggestionForCardResponse(suggestion?.Ideas ?? []);
    }
}
