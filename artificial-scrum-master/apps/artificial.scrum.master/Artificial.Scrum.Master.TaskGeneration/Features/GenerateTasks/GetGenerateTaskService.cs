using Artificial.Scrum.Master.TaskGeneration.Exceptions;
using Artificial.Scrum.Master.TaskGeneration.Features.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

namespace Artificial.Scrum.Master.TaskGeneration.Features.GenerateTasks
{
    public interface IGetGenerateTaskService
    {
        Task<GenerateTasksResponse> GenerateTasks(GenerateTasksRequest request);
    }
    internal class GetGenerateTaskService(ITaskGenerationService taskGenerationService) : IGetGenerateTaskService
    {
        private readonly ITaskGenerationService _taskGenerationService = taskGenerationService;

        public async Task<GenerateTasksResponse> GenerateTasks(GenerateTasksRequest request)
        {
            var tasks = await _taskGenerationService.GenerateTasks(request);

            if (!tasks.HasValue && tasks.Value.Tasks.Count == 0)
            {
                throw new GenerateTasksFailException(
                                       $"Generating tasks for story:{request.UserStoryTitle} failed");
            }

            return new GenerateTasksResponse
            {
                Tasks = tasks.Value.Tasks
            };
        }
    }
}
