using System.Text.Json;
using Artificial.Scrum.Master.EditTextSuggestions.Infrastructure;
using Artificial.Scrum.Master.EditTextSuggestions.Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace Artificial.Scrum.Master.Infrastructure.ExternalServices;

internal class OpenAIStorySuggestionService : IStorySuggestionService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IOpenAIService _openAIService;

    private readonly TimeSpan CacheTTL = TimeSpan.FromMinutes(45);
    private const int MaxTokens = 1000;

    public OpenAIStorySuggestionService(
        IMemoryCache memoryCache,
        IOpenAIService openAiService)
    {
        _memoryCache = memoryCache;
        _openAIService = openAiService;
    }

    public async Task<GetEditUserStorySuggestionResult?> GetEditUserStorySuggestion(
        string userStoryTitle,
        string userStoryDescription)
    {
        var cacheKey = $"{nameof(GetEditUserStorySuggestion)}_{userStoryTitle}_{userStoryDescription}";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheTTL;
            return await GetEditStorySuggestionInternal(userStoryTitle, userStoryDescription);
        });
    }

    private async Task<GetEditUserStorySuggestionResult?> GetEditStorySuggestionInternal(
        string userStoryTitle,
        string userStoryDescription)
    {
        var chat = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages =
            [
                ChatMessage.FromSystem(@"
Improve the description of the following UserStory or write one if none was provided.
The description must be written in Markdown.
Please return the description in json format:
{
    'StoryDescriptionSuggestion': 'As a user I want ...'
}
Please translate StoryDescriptionSuggestion to Polish and keep the Markdown format.
Try not to remove any information from the original description, especially URLs."),
                ChatMessage.FromSystem($"UserStory title: {userStoryTitle}"),
                ChatMessage.FromSystem($"UserStory description: {userStoryDescription}"),
            ],
            Model = OpenAIConsts.AIModel,
            MaxTokens = MaxTokens,
            ResponseFormat = new()
            {
                Type = StaticValues.CompletionStatics.ResponseFormat.Json
            },
        });

        var responseText = chat.Choices.FirstOrDefault()?.Message.Content;

        if (string.IsNullOrEmpty(responseText))
        {
            return null;
        }

        return JsonSerializer.Deserialize<GetEditUserStorySuggestionResult>(responseText);
    }
}
