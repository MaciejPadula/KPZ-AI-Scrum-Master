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
    private readonly IOpenAIService _openAIService;
    private const int MaxTokens = 250;

    public OpenAIRetroSuggestionService(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    public async Task<GetSuggestedIdeasResult?> GetSuggestedIdeas(IEnumerable<SessionCard> cards)
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
                ChatMessage.FromUser($"Good Cards: {string.Join(", ", cardsByType.GetValueOrDefault(CardType.Good, []))}"),
                ChatMessage.FromUser($"Bad Cards: {string.Join(", ", cardsByType.GetValueOrDefault(CardType.Bad, []))}"),
                ChatMessage.FromSystem(@"
You are participating in a retrospective meeting.
Please provide ideas on how to improve development process in future.
Please return response in json format:
{
    'Ideas': ['idea1', 'idea2', ...]
}
Return only few the most important ideas and try not to repeat ideas that are already in the list!!!!!!!!"),
                ChatMessage.FromUser($"Idea Cards To Avoid!!!!!!: {string.Join(", ", cardsByType.GetValueOrDefault(CardType.Ideas, []))}"),
                ChatMessage.FromSystem("Please try to return always at least three new unique ideas that are not present in the list."),
                ChatMessage.FromSystem("Please translate Ideas to Polish.")
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
