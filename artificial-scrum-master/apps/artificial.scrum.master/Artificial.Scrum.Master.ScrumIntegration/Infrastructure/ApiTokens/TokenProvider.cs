using Artificial.Scrum.Master.ScrumIntegration.Exceptions;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;

internal interface IAccessTokenProvider
{
    Task<string> ProvideRefreshTokenOrThrow(string userId);
}

internal class TokenProvider : IAccessTokenProvider
{
    private readonly IUserTokensRepository _userTokensRepository;

    public TokenProvider(IUserTokensRepository userTokensRepository)
    {
        _userTokensRepository = userTokensRepository;
    }

    public async Task<string> ProvideRefreshTokenOrThrow(string userId) =>
        (await _userTokensRepository.GetUserAccessTokens(userId))?.RefreshToken
            ?? throw new ProjectRequestForbidException($"Credentials of user:{userId} not found");
}
