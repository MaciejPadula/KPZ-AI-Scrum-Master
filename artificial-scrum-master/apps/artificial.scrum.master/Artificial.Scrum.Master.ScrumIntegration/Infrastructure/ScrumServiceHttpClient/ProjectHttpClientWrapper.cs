using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
using System.Net.Http.Json;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

internal class ProjectHttpClientWrapper : IProjectHttpClientWrapper
{
    private const string RefreshTokenUrl = "auth/refresh";

    private readonly HttpClient _httpClient;
    private readonly IUserTokensRepository _userTokensRepository;
    private readonly ITokenValidator _tokenValidator;

    public ProjectHttpClientWrapper(
        HttpClient httpClient,
        IUserTokensRepository userTokensRepository,
        ITokenValidator tokenValidator)
    {
        _httpClient = httpClient;
        _userTokensRepository = userTokensRepository;
        _tokenValidator = tokenValidator;
    }

    public async Task<TResponse> GetHttpRequest<TResponse>(
        string userId,
        UserTokens userTokens,
        string url) =>
            await SendRequest<TResponse>(
                userId,
                userTokens,
                () => _httpClient.GetAsync(url));

    public async Task<TResponse> PostHttpRequest<TRequest, TResponse>(
        string userId,
        UserTokens userTokens,
        string url,
        TRequest payload) =>
            await SendRequest<TResponse>(
                userId,
                userTokens,
                () => _httpClient.PostAsJsonAsync(url, payload));

    private async Task<TResponse> SendRequest<TResponse>(
        string userId,
        UserTokens userTokens,
        Func<Task<HttpResponseMessage>> messageSender)
    {
        if (!_tokenValidator.ValidateAccessTokenExpirationTime(userTokens.AccessToken))
        {
            userTokens = await RefreshUserTokens(userId, userTokens);
        }

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {userTokens.AccessToken}");
        var httpResponse = await messageSender();
        await EnsureStatusSuccess(httpResponse);

        return await httpResponse.Content.ReadFromJsonAsync<TResponse>()
            ?? throw new ProjectRequestFailedException("Response deserialization failed");
    }

    private async Task<UserTokens> RefreshUserTokens(string userId, UserTokens currentUserTokens)
    {
        if (!_tokenValidator.ValidateAccessTokenExpirationTime(currentUserTokens.RefreshToken))
        {
            throw new RefreshTokenExpiredException("Refresh token is expired, please log in");
        }

        var httpResponse = await _httpClient.PostAsJsonAsync(RefreshTokenUrl, new { refresh = currentUserTokens.RefreshToken });
        await EnsureStatusSuccess(httpResponse);

        var result = await httpResponse.Content.ReadFromJsonAsync<RefreshResponse>()
            ?? throw new ProjectRequestFailedException("Response deserialization failed");

        var newTokens = new UserTokens(
            result.AccessToken,
            result.RefreshToken);

        await _userTokensRepository.SaveAccessTokensWhenExists(userId, newTokens);
        return newTokens;
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
