using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;
using System.Net.Http.Json;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

internal class ProjectHttpClientWrapper : IProjectHttpClientWrapper
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IJwtDecoder _jwtDecoder;
    private readonly ITokenRefresher _tokenRefresher;

    public ProjectHttpClientWrapper(
        IHttpClientFactory httpClientFactory,
        IJwtDecoder jwtDecoder,
        ITokenRefresher tokenRefresher)
    {
        _httpClientFactory = httpClientFactory;
        _jwtDecoder = jwtDecoder;
        _tokenRefresher = tokenRefresher;
    }

    public async Task<TResponse> GetHttpRequest<TResponse>(
        string userId,
        string refreshToken,
        Func<UserDetails, string> urlFactory) =>
        await SendRequest<TResponse>(
            userId,
            refreshToken,
            (httpClient, user) => httpClient.GetAsync(urlFactory(user)));

    public async Task<TResponse> ResourceUpdateHttpRequest<TRequest, TResponse>(
        ResourceUpdateHttpMethod httpMethod,
        string userId,
        string refreshToken,
        Func<UserDetails, string> urlFactory,
        TRequest payload)
    {
        var request = httpMethod switch
        {
            ResourceUpdateHttpMethod.POST =>
                new Func<HttpClient, UserDetails, Task<HttpResponseMessage>>(
                    (httpClient, user) => httpClient.PostAsJsonAsync(urlFactory(user), payload)),
            ResourceUpdateHttpMethod.PUT =>
                (httpClient, user) => httpClient.PutAsJsonAsync(urlFactory(user), payload),
            ResourceUpdateHttpMethod.PATCH =>
                (httpClient, user) => httpClient.PatchAsJsonAsync(urlFactory(user), payload),
            _ => throw new ProjectRequestFailedException("Unsupported http method")
        };

        return await SendRequest<TResponse>(
            userId,
            refreshToken,
            request);
    }

    private async Task<TResponse> SendRequest<TResponse>(
        string userId,
        string refreshToken,
        Func<HttpClient, UserDetails, Task<HttpResponseMessage>> messageSender)
    {
        var userTokens = await _tokenRefresher.RefreshUserTokens(userId, refreshToken);

        var memberId = _jwtDecoder.GetClaim(userTokens.AccessToken, "user_id");
        if (string.IsNullOrEmpty(memberId))
        {
            throw new ProjectRequestForbidException("User id not found in token");
        }

        using var httpClient = _httpClientFactory.CreateClient(Consts.TaigaClient);
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {userTokens.AccessToken}");

        var httpResponse = await messageSender(httpClient, new(memberId));
        await EnsureStatusSuccess(httpResponse);

        return await httpResponse.Content.ReadFromJsonAsync<TResponse>()
               ?? throw new ProjectRequestFailedException("Response deserialization failed");
    }

    private static async Task EnsureStatusSuccess(HttpResponseMessage httpResponse)
    {
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync();
            throw new ProjectRequestFailedException(
                $"Request failed with status code {httpResponse.StatusCode}: {errorContent}");
        }
    }
}
