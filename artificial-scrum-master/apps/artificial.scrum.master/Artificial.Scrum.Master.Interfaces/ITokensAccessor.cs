namespace Artificial.Scrum.Master.Interfaces;

public interface ITokensAccessor
{
    UserTokens? GetUserTokens(string userId);
}