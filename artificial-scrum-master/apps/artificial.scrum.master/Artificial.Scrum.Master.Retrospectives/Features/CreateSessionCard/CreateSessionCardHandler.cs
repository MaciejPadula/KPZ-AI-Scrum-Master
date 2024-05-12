using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using Artificial.Scrum.Master.SharedKernel;

namespace Artificial.Scrum.Master.Retrospectives.Features.CreateSessionCard;

internal interface ICreateSessionCardHandler
{
    Task<Result> Handle(CreateSessionCardRequest request);
}

internal class CreateSessionCardHandler : ICreateSessionCardHandler
{
    private readonly IRetroSessionRepository _retroSessionRepository;
    private readonly TimeProvider _timeProvider;

    public CreateSessionCardHandler(
        IRetroSessionRepository retroSessionRepository,
        TimeProvider timeProvider)
    {
        _retroSessionRepository = retroSessionRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result> Handle(CreateSessionCardRequest request)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult is not null)
        {
            return Result.OnError(validationResult);
        }

        await _retroSessionRepository.CreateSessionCard(new(
            request.Content,
            request.Type,
            request.SessionId,
            _timeProvider.GetUtcNow().DateTime));

        return Result.OnSuccess();
    }

    private static Exception? ValidateRequest(CreateSessionCardRequest request)
    {
        if (!CardTypes.Any(x => request.Type == x))
        {
            return new InvalidCardTypeException(request.Type);
        }

        return null;
    }

    private static readonly IEnumerable<CardType> CardTypes = Enum.GetValues<CardType>();
}
