using Artificial.Scrum.Master.ScrumProjectIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ApiTokens;
using System.Net;
using System.Text.Json;
using Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.ScrumServiceHttpClient;
using Artificial.Scrum.Master.ScrumProjectIntegration.Utilities;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects
{
    internal interface IGetUserProjectsService
    {
        // TODO: do zmainy, trzeba bedzie autoryzowac uzytkownika jakims wlasnym tokenem JWT
        Task<IEnumerable<GetUserProjectsResponse>> Handle(string userId);
    }

    internal class GetUserProjectsService : IGetUserProjectsService
    {
        // TODO: wrapper opakowujacy http clienta powinien brac uuid uzytkownika, nastepnie wyciagnac z bazy tokeny do API Taiga.io...\

        private readonly IUserTokensRepository _userTokensRepository;
        private readonly IProjectHttpClientWrapper _projectHttpClientWrapper;
        private readonly IJwtDecoder _jwtDecoder;

        public GetUserProjectsService(IUserTokensRepository userTokensRepository,
            IProjectHttpClientWrapper projectHttpClientWrapper, IJwtDecoder jwtDecoder)
        {
            _userTokensRepository = userTokensRepository;
            _projectHttpClientWrapper = projectHttpClientWrapper;
            _jwtDecoder = jwtDecoder;
        }

        public async Task<IEnumerable<GetUserProjectsResponse>> Handle(string userId)
        {
            // TODO: tutaj wyciagniecie z bazy tokenow po id...
            var userTokens = await _userTokensRepository.GetUserAccessTokens(userId);
            if (userTokens is null)
            {
                throw new ProjectResourceNotFoundException($"Credentials of user:{userId} not found");
            }

            var projectRequestResult = await _projectHttpClientWrapper.GetHttpRequest(
                userId,
                $"projects?member={_jwtDecoder.GetClaim(userTokens.Value.AccessToken, "user_id")}"
            );

            if (projectRequestResult.StatusCode != HttpStatusCode.OK)
            {
                throw new ProjectRequestFailedException(projectRequestResult.Response);
            }

            var serializedResponse = JsonSerializer.Deserialize<List<Project>>(projectRequestResult.Response);
            if (serializedResponse is null)
            {
                throw new ProjectRequestFailedException(projectRequestResult.Response);
            }

            return serializedResponse.Select(project => new GetUserProjectsResponse
            {
                Id = project.Id,
                Name = project.Name,
                Slug = project.Slug,
                Description = project.Description,
                ModifiedDate = project.ModifiedDate,
                OwnerUsername = project.Owner.Username,
                IsPrivate = project.IsPrivate
            }).ToList();
        }
    }
}
