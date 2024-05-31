using Artificial.Scrum.Master.Prioritization.Infrastructure;
using Artificial.Scrum.Master.Prioritization.Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using System.Text.Json;
using OpenAI.ObjectModels;

namespace Artificial.Scrum.Master.Infrastructure.ExternalServices;

internal class OpenAIStoryPrioritySuggestionService : IStoryPrioritizationSuggestionService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IOpenAIService _openAIService;

    private readonly TimeSpan CacheTTL = TimeSpan.FromMinutes(45);
    private const int MaxTokens = 1000;

    public OpenAIStoryPrioritySuggestionService(IMemoryCache memoryCache, IOpenAIService openAiService)
    {
        _memoryCache = memoryCache;
        _openAIService = openAiService;
    }

    public Task<GetStoryPrioritizationSuggestionResult?> GetStoryPrioritizationSuggestion(
        StoriesPrioritySuggestionRequest suggestionRequest, bool generateAgain)
    {
        var cacheKey =
            $"{nameof(GetStoryPrioritizationSuggestion)}_{string.Join("_", suggestionRequest.Stories.Select(x => x.Title))}";
        if (generateAgain)
        {
            _memoryCache.Remove(cacheKey);
        }

        return _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheTTL;
            return await GetStoryPrioritizationSuggestionInternal(suggestionRequest);
        });
    }

    private async Task<GetStoryPrioritizationSuggestionResult?> GetStoryPrioritizationSuggestionInternal(
        StoriesPrioritySuggestionRequest suggestionRequest)
    {
        var storiesWithTasks = JsonSerializer.Serialize(suggestionRequest.Stories);
        var chat = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages =
            [
                ChatMessage.FromSystem(@"
You are asked to suggest the priority of UserStories of a Sprint in a project.
Sort the UserStories by priority from highest to least.
Please provide the priority of the following UserStories in json format:
{
    'UserStories': [
        {
            'Title': 'UserStory1',
            'UserStoryId': 1,
        },
        {
            'Title': 'UserStory2',
            'UserStoryId': 2,
        },
        ...
    ]
}"),
                ChatMessage.FromUser(
                    "You have the following UserStories with UserStoryIds, Titles and their tasks, provided in json:"),
                ChatMessage.FromUser(storiesWithTasks),
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

        return JsonSerializer.Deserialize<GetStoryPrioritizationSuggestionResult>(responseText);
    }
}
