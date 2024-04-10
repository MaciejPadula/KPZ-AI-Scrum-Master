using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;
using Artificial.Scrum.Master.ScrumIntegration.Mappers.TimelineEvents;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

internal interface ITimeLineEventParser
{
    GetProfileTimeLineResponse ParseProfileTimeLineElement(IEnumerable<TimeLineEventRoot> elements);
    GetProjectTimeLineResponse ParseProjectTimeLineElement(IEnumerable<TimeLineEventRoot> elements);
}

internal class TimeLineEventParser : ITimeLineEventParser
{
    private readonly ITimeLineEventObjectsMapper _timeLineEventObjectMapper;

    public TimeLineEventParser(ITimeLineEventObjectsMapper timeLineEventObjectMapper)
    {
        _timeLineEventObjectMapper = timeLineEventObjectMapper;
    }

    public GetProfileTimeLineResponse ParseProfileTimeLineElement(IEnumerable<TimeLineEventRoot> elements)
    {
        var timelineEvents = elements.Select(elem =>
        {
            var parsedValuesDiff = _timeLineEventObjectMapper.ParseValuesDiff(elem.Data.ValuesDiff);
            if (!string.IsNullOrEmpty(elem.Data.Comment))
            {
                parsedValuesDiff.Add(new KeyValuePair<string, string>("Comment", elem.Data.Comment));
            }

            var (scrumObjectType, scrumObjectState) = _timeLineEventObjectMapper.ParseEventTypeEnum(elem.EventType);
            var (objectId, objectName) = _timeLineEventObjectMapper.ParseScrumObject(scrumObjectType, elem.Data.Task,
                elem.Data.Userstory, elem.Data.Milestone, elem.Data.User, elem.Data.Project);

            return new GetTimeLineEvent
            {
                EventId = elem.Id,
                ScrumObjectType = scrumObjectType,
                ScrumObjectState = scrumObjectState,
                Created = elem.Created,
                ProjectId = elem.Project ?? -1,
                ProjectName = elem.Data.Project?.Name ?? string.Empty,
                ObjectId = objectId,
                ObjectName = objectName,
                UserId = elem.Data.User.Id,
                UserName = elem.Data.User.Name,
                UserPhoto = elem.Data.User.Photo,
                UserNick = elem.Data.User.Username,
                ValuesDiff = parsedValuesDiff
            };
        }).ToList();

        return new GetProfileTimeLineResponse { TimeLineEvents = timelineEvents };
    }

    public GetProjectTimeLineResponse ParseProjectTimeLineElement(IEnumerable<TimeLineEventRoot> elements)
    {
        var timelineEvents = elements.Select(elem =>
        {
            var parsedValuesDiff = _timeLineEventObjectMapper.ParseValuesDiff(elem.Data.ValuesDiff);
            if (!string.IsNullOrEmpty(elem.Data.Comment))
            {
                parsedValuesDiff.Add(new KeyValuePair<string, string>("Comment", elem.Data.Comment));
            }

            var (scrumObjectType, scrumObjectState) = _timeLineEventObjectMapper.ParseEventTypeEnum(elem.EventType);
            var (objectId, objectName) = _timeLineEventObjectMapper.ParseScrumObject(scrumObjectType, elem.Data.Task,
                elem.Data.Userstory, elem.Data.Milestone, elem.Data.User, elem.Data.Project);

            return new GetTimeLineEvent
            {
                EventId = elem.Id,
                ScrumObjectType = scrumObjectType,
                ScrumObjectState = scrumObjectState,
                Created = elem.Created,
                ProjectId = elem.Project ?? -1,
                ObjectId = objectId,
                ObjectName = objectName,
                UserId = elem.Data.User.Id,
                UserName = elem.Data.User.Name,
                UserPhoto = elem.Data.User.Photo,
                UserNick = elem.Data.User.Username,
                ProjectName = elem.Data.Project?.Name ?? string.Empty,
                ValuesDiff = parsedValuesDiff
            };
        }).ToList();

        return new GetProjectTimeLineResponse { TimeLineEvents = timelineEvents };
    }
}
