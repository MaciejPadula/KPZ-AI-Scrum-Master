using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ScrumServiceHttpClient;

public interface IProjectHttpClientWrapper
{
    Task<TResponse> GetHttpRequest<TResponse>(string userId, UserTokens userTokens, string url);

    Task<TResponse> PostHttpRequest<TRequest, TResponse>(
        string userId, UserTokens userTokens, string url, TRequest payload);
}
