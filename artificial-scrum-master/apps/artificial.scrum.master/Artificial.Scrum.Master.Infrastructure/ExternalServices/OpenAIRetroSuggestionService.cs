using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;
using Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using System.Text.Json;

namespace Artificial.Scrum.Master.Infrastructure.ExternalServices;

internal class OpenAIRetroSuggestionService : IRetroSuggestionService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IOpenAIService _openAIService;

    private readonly TimeSpan CacheTTL = TimeSpan.FromMinutes(45);
    private const int MaxTokens = 250;

    public OpenAIRetroSuggestionService(
        IMemoryCache memoryCache,
        IOpenAIService openAIService)
    {
        _memoryCache = memoryCache;
        _openAIService = openAIService;
    }

    public async Task<GetSuggestedIdeasResult?> GetSuggestedIdeas(IEnumerable<SessionCard> cards)
    {
        var cacheKey = $"{nameof(GetSuggestedIdeas)}_[{string.Join(",", cards.Select(x => $"{x.Content}:{x.Type}"))}]";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheTTL;
            return await GetSuggestedIdeasInternal(cards);
        });
    }


    public async Task<GetSuggestedIdeasResult?> GetSuggestedIdeasInternal(IEnumerable<SessionCard> cards)
    {
        var cardsByType = cards
            .GroupBy(x => x.Type)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Content).ToList());

        var chat = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages =
            [
                ChatMessage.FromSystem(@"
You are participating in a retrospective meeting. You have the following cards:"),
                ChatMessage.FromSystem($"Good Cards: {string.Join(" ,", cardsByType.GetValueOrDefault(CardType.Good, []))}"),
                ChatMessage.FromSystem($"Bad Cards: {string.Join(" ,", cardsByType.GetValueOrDefault(CardType.Bad, []))}"),
                ChatMessage.FromSystem($"Idea Cards: {string.Join(" ,", cardsByType.GetValueOrDefault(CardType.Ideas, []))}"),
                ChatMessage.FromSystem(@"
Please provide ideas on how to improve the following tasks.
Please return response in json format:
{
    'Ideas': ['idea1', 'idea2', ...]
}
Please translate Ideas to Polish.")
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

        return JsonSerializer.Deserialize<GetSuggestedIdeasResult>(responseText);
    }
}
