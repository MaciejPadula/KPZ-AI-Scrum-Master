
namespace Artificial.Scrum.Master.TokenRefresher.Interfaces;

public interface ITokenRepository
{
    Task<Dictionary<string, string>> GetAllAccessTokens();
    Task UpdateToken(string userId, RefreshResponse result);
}
