using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
using Artificial.Scrum.Master.ScrumIntegration.Utilities;
using System.Net.Http.Json;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

internal class ProjectHttpClientWrapper : IProjectHttpClientWrapper
{
    private readonly HttpClient _httpClient;
    private readonly IJwtDecoder _jwtDecoder;
    private readonly ITokenRefresher _tokenRefresher;

    public ProjectHttpClientWrapper(
        HttpClient httpClient,
        IJwtDecoder jwtDecoder,
        ITokenRefresher tokenRefresher)
    {
        _httpClient = httpClient;
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
                user => _httpClient.GetAsync(urlFactory(user)));

    public async Task<TResponse> PostHttpRequest<TRequest, TResponse>(
        string userId,
        string refreshToken,
        Func<UserDetails, string> urlFactory,
        TRequest payload) =>
            await SendRequest<TResponse>(
                userId,
                refreshToken,
                user => _httpClient.PostAsJsonAsync(urlFactory(user), payload));

    private async Task<TResponse> SendRequest<TResponse>(
        string userId,
        string refreshToken,
        Func<UserDetails, Task<HttpResponseMessage>> messageSender)
    {
        var userTokens = await _tokenRefresher.RefreshUserTokens(userId, refreshToken);

        var memberId = _jwtDecoder.GetClaim(userTokens.AccessToken, "user_id");
        if (string.IsNullOrEmpty(memberId))
        {
            throw new ProjectRequestForbidException("User id not found in token");
        }

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {userTokens.AccessToken}");
        var httpResponse = await messageSender(new(memberId));
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
