using Artificial.Scrum.Master.EditTextSuggestions.Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using System.Text.Json;
using Artificial.Scrum.Master.EditTextSuggestions.Infrastructure;
using OpenAI.ObjectModels;

namespace Artificial.Scrum.Master.Infrastructure.ExternalServices;

internal class OpenAITaskSuggestionService(
    IMemoryCache memoryCache,
    IOpenAIService openAiService) : ITaskSuggestionService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IOpenAIService _openAIService = openAiService;

    private readonly TimeSpan CacheTTL = TimeSpan.FromMinutes(45);
    private const int MaxTokens = 1000;

    public async Task<GetEditTaskSuggestionResult?> GetEditTaskSuggestion(
        string userStoryTitle,
        string taskTitle,
        string taskDescription)
    {
        var cacheKey = $"{nameof(GetEditTaskSuggestion)}_{taskTitle}_{taskDescription}";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheTTL;
            return await GetEditTaskSuggestionInternal(userStoryTitle, taskTitle, taskDescription);
        });
    }

    private async Task<GetEditTaskSuggestionResult?> GetEditTaskSuggestionInternal(
        string userStoryTitle,
        string taskTitle,
        string taskDescription)
    {
        var chat = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages =
            [
                ChatMessage.FromSystem(@"
Improve the description of the following task or write one if none was provided.
The description must be written in Markdown.
Please return the description in json format:
{
    'TaskDescriptionSuggestion': 'We need ...'
}
Please translate TaskDescriptionSuggestion to Polish and keep the Markdown format.
Try not to remove any information from the original description, especially URLs."),
                ChatMessage.FromSystem($"Task belongs to User Story: {userStoryTitle}"),
                ChatMessage.FromSystem($"Task title: {taskTitle}"),
                ChatMessage.FromSystem($"Task description: {taskDescription}"),
            ],
            Model = Models.Gpt_3_5_Turbo,
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

        return JsonSerializer.Deserialize<GetEditTaskSuggestionResult>(responseText);
    }
}
