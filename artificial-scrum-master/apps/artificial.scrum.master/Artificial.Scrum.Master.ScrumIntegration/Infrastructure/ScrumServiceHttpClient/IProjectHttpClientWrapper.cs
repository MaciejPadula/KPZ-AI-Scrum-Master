using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

internal interface IProjectHttpClientWrapper
{
    Task<TResponse> GetHttpRequest<TResponse>(string userId, string refreshToken, Func<UserDetails, string> urlFactory);

    Task<TResponse> PostHttpRequest<TRequest, TResponse>(string userId, string refreshToken, Func<UserDetails, string> urlFactory, TRequest payload);

    Task<TResponse> PatchHttpRequest<TRequest, TResponse>(string userId, string refreshToken, Func<UserDetails, string> urlFactory, TRequest payload);
}
