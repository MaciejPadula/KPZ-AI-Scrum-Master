using Artificial.Scrum.Master.TaskGeneration.Features.GenerateTasks;
using Artificial.Scrum.Master.TaskGeneration.Features.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using System.Text.Json;

namespace Artificial.Scrum.Master.Infrastructure.ExternalServices;
internal class OpenAITaskGenerationService : ITaskGenerationService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IOpenAIService _openAIService;

    private readonly TimeSpan CacheTTL = TimeSpan.FromMinutes(45);
    private const int MaxTokens = 1000;

    public OpenAITaskGenerationService(
               IMemoryCache memoryCache,
                      IOpenAIService openAiService)
    {
        _memoryCache = memoryCache;
        _openAIService = openAiService;
    }

    public async Task<GenerateTasksResponse?> GenerateTasks(GenerateTasksRequest request)
    {
        var cacheKey = $"{nameof(GenerateTasks)}_{request.UserStoryTitle}_{request.UserStoryDescription}";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheTTL;
            return await GenerateTasksInternal(request);
        });
    }

    private async Task<GenerateTasksResponse?> GenerateTasksInternal(GenerateTasksRequest request)
    {
        var chat = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages =
            [
                ChatMessage.FromSystem(@"
Based on provided title and description of a User Story create individual tasks.
The description must be written in Markdown.
Return the tasks in json format:
{
    'Tasks': [
        {
            'Title': 'Task 1',
            'Description': 'Task 1 Description ...'
        },
        {
            'Title': 'Task 2',
            'Description': 'Task 2 Description ...'
        }
    ]
}
Translate Title and Description to Polish and keep the Markdown format.
Do not remove any information from the original description, especially URLs."),
                ChatMessage.FromSystem($"User Story title: {request.UserStoryTitle}"),
                ChatMessage.FromSystem($"User Story description: {request.UserStoryDescription}"),
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

        return JsonSerializer.Deserialize<GenerateTasksResponse>(responseText);
    }
}
