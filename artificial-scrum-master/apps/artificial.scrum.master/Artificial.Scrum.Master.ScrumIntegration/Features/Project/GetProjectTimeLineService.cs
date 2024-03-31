namespace Artificial.Scrum.Master.ScrumIntegration.Features.Project;

internal interface IGetProjectTimeLineService
{
    Task<GetProjectTimeLineResponse> Handle(string userId, string projectId);
}

internal class GetProjectTimeLineService : IGetProjectTimeLineService
{
    public Task<GetProjectTimeLineResponse> Handle(string userId, string projectId)
    {
        throw new NotImplementedException();
    }
}
