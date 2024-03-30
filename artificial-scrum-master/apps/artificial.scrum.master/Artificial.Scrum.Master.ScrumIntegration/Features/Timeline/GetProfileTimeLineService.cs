namespace Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

internal interface IGetProfileTimeLineService
{
    Task<GetProfileTimeLineResponse> Handle(string userId);
}

internal class GetProfileTimeLineService : IGetProfileTimeLineService
{
    public Task<GetProfileTimeLineResponse> Handle(string userId)
    {
        throw new NotImplementedException();
    }
}
