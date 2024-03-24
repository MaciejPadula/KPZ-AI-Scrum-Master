using Artificial.Scrum.Master.ScrumProjectIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumProjectIntegration.Settings;
using System.Net;
using System.Runtime;
using System.Text.Json;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects
{
    internal interface IGetUserProjectsService
    {
        // TODO: do zmainy, trzeba bedzie autoryzowac uzytkownika jakims wlasnym tokenem JWT
        Task<GetUserProjectsResponse> Handle(string userId);
    }

    internal class GetUserProjectsService : IGetUserProjectsService
    {
        // TODO: wrapper opakowujacy http clienta powinien brac uuid uzytkownika, nastepnie wyciagnac z bazy tokeny do API Taiga.io...\

        private readonly IUserTokensRepository _userTokensRepository;
        private readonly ScrumManagementServiceSettings _settings;

        public GetUserProjectsService(IUserTokensRepository userTokensRepository,
            ScrumManagementServiceSettings settings)
        {
            _userTokensRepository = userTokensRepository;
            _settings = settings;
        }

        public async Task<GetUserProjectsResponse> Handle(string userId)
        {
            // TODO: tutaj wyciagniecie z bazy tokenow po id...
            var userTokens = await _userTokensRepository.GetUserAccessToken(userId);

            using var client = new HttpClient();


            var response = await client.GetAsync($"{_settings.BaseUrl}/projects");
            var responseString = await response.Content.ReadAsStringAsync();

            var serializedResponse = JsonSerializer.Deserialize<List<Project>>(responseString);
            if (response.StatusCode != HttpStatusCode.OK || serializedResponse is null)
            {
                throw new RequestFailedException(response.StatusCode, responseString);
            }

            return new GetUserProjectsResponse();
        }
    }
}
