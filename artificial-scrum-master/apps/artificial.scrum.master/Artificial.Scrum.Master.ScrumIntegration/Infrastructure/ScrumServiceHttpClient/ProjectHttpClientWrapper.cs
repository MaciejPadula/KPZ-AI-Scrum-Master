using Artificial.Scrum.Master.ScrumIntegration.Exceptions;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ApiTokens;
using Artificial.Scrum.Master.ScrumIntegration.Infrastructure.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Artificial.Scrum.Master.ScrumIntegration.Infrastructure.ScrumServiceHttpClient;

public class ProjectHttpClientWrapper : IProjectHttpClientWrapper
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
        string userId, UserTokens userTokens, string url)
    {
        var isValid = _tokenValidator.ValidateAccessTokenExpirationTime(userTokens.AccessToken);
        if (!isValid)
        {
            userTokens = await RefreshUserTokens(userId, userTokens);
        }

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {userTokens.AccessToken}");
        var httpResponse = await _httpClient.GetAsync(url);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync();
            throw new ProjectResourceNotFoundException(
                $"Request to {url} failed with status code {httpResponse.StatusCode}: {errorContent}");
        }

        var result = await httpResponse.Content.ReadFromJsonAsync<TResponse>();
        if (result is null)
        {
            throw new ProjectRequestFailedException("Response deserialization failed");
        }

        return result;
    }

    public async Task<TResponse> PostHttpRequest<TRequest, TResponse>(
        string userId, UserTokens userTokens, string url, TRequest payload)
    {
        var isValid = _tokenValidator.ValidateAccessTokenExpirationTime(userTokens.AccessToken);
        if (!isValid)
        {
            userTokens = await RefreshUserTokens(userId, userTokens);
        }

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {userTokens.AccessToken}");
        var httpResponse = await _httpClient.PostAsJsonAsync(url, payload);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync();
            throw new ProjectResourceNotFoundException(
                $"Request to {url} failed with status code {httpResponse.StatusCode}: {errorContent}");
        }

        var result = await httpResponse.Content.ReadFromJsonAsync<TResponse>();
        if (result is null)
        {
            throw new ProjectRequestFailedException("Response deserialization failed");
        }

        return result;
    }

    private async Task<UserTokens> RefreshUserTokens(string userId, UserTokens currentUserTokens)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync(RefreshTokenUrl, new
        {
            refresh = currentUserTokens.RefreshToken
        });

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync();
            throw new ProjectResourceNotFoundException(
                $"Request to refresh jwt: {RefreshTokenUrl} failed with status code {httpResponse.StatusCode}: {errorContent}");
        }

        var result = await httpResponse.Content.ReadFromJsonAsync<RefreshResponse>();
        if (result is null)
        {
            throw new ProjectRequestFailedException("Response deserialization failed");
        }

        var newTokens = new UserTokens
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken
        };

        await _userTokensRepository.SaveAccessTokens(userId, JsonSerializer.Serialize(newTokens));
        return newTokens;
    }
}
