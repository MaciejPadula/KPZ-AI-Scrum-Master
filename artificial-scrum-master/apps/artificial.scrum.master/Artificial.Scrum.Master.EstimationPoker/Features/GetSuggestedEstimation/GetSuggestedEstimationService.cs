using Artificial.Scrum.Master.EstimationPoker.Features.Shared.Exceptions;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.SharedKernel;

namespace Artificial.Scrum.Master.EstimationPoker.Features.GetSuggestedEstimation;

internal interface IGetSuggestedEstimationService
{
    Task<Result<GetSuggestedEstimationResponse>> Handle(GetSuggestedEstimationRequest request);
}

internal class GetSuggestedEstimationService : IGetSuggestedEstimationService
{
    private readonly IPokerSuggestionService _pokerSuggestionService;
    private readonly ISessionTaskRepository _sessionTaskRepository;
    private readonly IUserAccessor _userAccessor;
    private readonly ISessionRepository _sessionRepository;

    public GetSuggestedEstimationService(
        IPokerSuggestionService pokerSuggestionService,
        ISessionTaskRepository sessionTaskRepository,
        IUserAccessor userAccessor,
        ISessionRepository sessionRepository)
    {
        _pokerSuggestionService = pokerSuggestionService;
        _sessionTaskRepository = sessionTaskRepository;
        _userAccessor = userAccessor;
        _sessionRepository = sessionRepository;
    }

    public async Task<Result<GetSuggestedEstimationResponse>> Handle(GetSuggestedEstimationRequest request)
    {
        var userId = _userAccessor.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<GetSuggestedEstimationResponse>.OnError(new UnauthorizedAccessException());
        }
        var task = await _sessionTaskRepository.GetTaskById(request.TaskId);

        if (!task.HasValue)
        {
            return Result<GetSuggestedEstimationResponse>.OnError(new TaskNotFoundException(request.TaskId));
        }

        if (!await _sessionRepository.ValidateUserAccess(userId, task.Value.SessionId))
        {
            return Result<GetSuggestedEstimationResponse>.OnError(new UserNotAuthorizedException(userId, task.Value.SessionId));
        }

        var estimation = await _pokerSuggestionService.GetSuggestedEstimation(task.Value.Title, task.Value.Description, request.Estimations);

        if (!estimation.HasValue)
        {
            return Result<GetSuggestedEstimationResponse>.OnError(new SuggestionServiceErrorException());
        }

        return Result<GetSuggestedEstimationResponse>.OnSuccess(new(
            estimation.Value.Value,
            estimation.Value.Reason));
    }
}
