using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Models;
using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using System.Text.Json;

namespace Artificial.Scrum.Master.Infrastructure.ExternalServices;

internal class OpenAIPokerSuggestionService : IPokerSuggestionService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IOpenAIService _openAIService;

    private readonly TimeSpan CacheTTL = TimeSpan.FromMinutes(45);
    private const int MaxTokens = 250;

    public OpenAIPokerSuggestionService(
        IMemoryCache memoryCache,
        IOpenAIService openAIService)
    {
        _memoryCache = memoryCache;
        _openAIService = openAIService;
    }

    public async Task<GetSuggestedEstimationResult?> GetSuggestedEstimation(string taskTitle, string taskDescription, List<decimal> estimations)
    {
        var cacheKey = $"{nameof(GetSuggestedEstimation)}_{taskTitle}_{taskDescription}_{string.Join("-", estimations)}";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheTTL;
            return await GetSuggestedEstimationInternal(taskTitle, taskDescription, estimations);
        });
    }

    private async Task<GetSuggestedEstimationResult?> GetSuggestedEstimationInternal(string taskTitle, string taskDescription, List<decimal> estimations)
    {
        var chat = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages =
            [
                ChatMessage.FromSystem(@"
Estimate time that is required to complete the following task and return it in json format:
{
    'Value': 5,
    'Reason': 'I chose this value because...'
}
Please translate Reason to Polish"),
                ChatMessage.FromSystem($"Task title: {taskTitle}"),
                ChatMessage.FromSystem($"Task description: {taskDescription}"),
                ChatMessage.FromSystem("Estimations:"),
                ChatMessage.FromUser(string.Join(", ", estimations))
            ],
            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo,
            MaxTokens = MaxTokens,
            ResponseFormat = new()
            {
                Type = StaticValues.CompletionStatics.ResponseFormat.Json
            },
        });

        var responseText = chat.Choices?.FirstOrDefault()?.Message?.Content;

        if (string.IsNullOrEmpty(responseText))
        {
            return null;
        }

        return JsonSerializer.Deserialize<GetSuggestedEstimationResult>(responseText);
    }
}
