using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ScrumServiceHttpClient
{
    public interface IProjectHttpClientWrapper
    {
        Task<WrapperHttpResponse> GetHttpRequest(string userId, string url);
        Task<WrapperHttpResponse> PostHttpRequest(string userId, string url, Dictionary<string, string> parameters);
    }
}
