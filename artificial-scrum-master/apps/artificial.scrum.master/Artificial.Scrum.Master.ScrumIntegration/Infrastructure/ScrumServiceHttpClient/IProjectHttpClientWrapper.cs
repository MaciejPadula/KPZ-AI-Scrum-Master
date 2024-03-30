using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

public interface IProjectHttpClientWrapper
{
    Task<TResponse> GetHttpRequest<TResponse>(string userId, UserTokens userTokens, string url);

    Task<TResponse> PostHttpRequest<TRequest, TResponse>(
        string userId, UserTokens userTokens, string url, TRequest payload);
}
