using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

internal interface IProjectHttpClientWrapper
{
    Task<TResponse> GetHttpRequest<TResponse>(string userId, string refreshToken, Func<UserDetails, string> urlFactory);

    Task<TResponse> ResourceUpdateHttpRequest<TRequest, TResponse>(
        ResourceUpdateHttpMethod httpMethod, string userId,
        string refreshToken, Func<UserDetails, string> urlFactory, TRequest payload);
}
