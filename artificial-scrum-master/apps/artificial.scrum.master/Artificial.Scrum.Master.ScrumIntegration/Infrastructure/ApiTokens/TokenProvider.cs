using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;

internal interface IAccessTokenProvider
{
    Task<UserTokens> ProvideOrThrow(string userId);
}

internal class TokenProvider : IAccessTokenProvider
{
    private readonly IUserTokensRepository _userTokensRepository;

    public TokenProvider(IUserTokensRepository userTokensRepository)
    {
        _userTokensRepository = userTokensRepository;
    }

    public async Task<UserTokens> ProvideOrThrow(string userId) =>
        await _userTokensRepository.GetUserAccessTokens(userId)
            ?? throw new ProjectRequestForbidException($"Credentials of user:{userId} not found");
}
