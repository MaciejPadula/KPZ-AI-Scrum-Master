using Artificial.Scrum.Master.EditTextSuggestions.Exceptions;
using Artificial.Scrum.Master.EditTextSuggestions.Infrastructure;

namespace Artificial.Scrum.Master.EditTextSuggestions.Features.GetEditStorySuggestion;

internal interface IGetEditStorySuggestionService
{
    Task<GetEditStorySuggestionResponse> Handle(GetEditStorySuggestionRequest request);
}

internal class GetEditStorySuggestionService : IGetEditStorySuggestionService
{
    private readonly IStorySuggestionService _storySuggestionService;

    public GetEditStorySuggestionService(IStorySuggestionService storySuggestionService)
    {
        _storySuggestionService = storySuggestionService;
    }

    public async Task<GetEditStorySuggestionResponse> Handle(GetEditStorySuggestionRequest request)
    {
        var suggestion =
            await _storySuggestionService.GetEditUserStorySuggestion(
                request.StoryTitle,
                request.StoryDescription ?? string.Empty);

        if (!suggestion.HasValue)
        {
            throw new GenerateSuggestionFailException(
                $"Generating edit suggestion for UserStory:{request.StoryTitle} failed");
        }

        return new GetEditStorySuggestionResponse(request.StoryTitle, suggestion.Value.StoryDescriptionSuggestion);
    }
}
