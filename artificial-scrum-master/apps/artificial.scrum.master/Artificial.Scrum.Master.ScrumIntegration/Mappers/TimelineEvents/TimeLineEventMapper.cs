using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;

namespace Artificial.Scrum.Master.ScrumIntegration.Mappers.TimelineEvents;

internal interface ITimeLineEventMapper
{
    GetProfileTimeLineResponse ParseProfileTimeLineElement(IEnumerable<TimeLineEventRoot> elements);
    GetProjectTimeLineResponse ParseProjectTimeLineElement(IEnumerable<TimeLineEventRoot> elements);
}

internal class TimeLineEventMapper : ITimeLineEventMapper
{
    private readonly ITimeLineEventObjectsParser _timeLineEventObjectParser;

    public TimeLineEventMapper(ITimeLineEventObjectsParser timeLineEventObjectParser)
    {
        _timeLineEventObjectParser = timeLineEventObjectParser;
    }

    public GetProfileTimeLineResponse ParseProfileTimeLineElement(IEnumerable<TimeLineEventRoot> elements)
    {
        var timelineEvents = elements.Select(elem =>
        {
            var parsedValuesDiff = _timeLineEventObjectParser.ParseValuesDiff(elem.Data.ValuesDiff);
            if (!string.IsNullOrEmpty(elem.Data.Comment))
            {
                parsedValuesDiff.Add(new KeyValuePair<string, string>("Comment", elem.Data.Comment));
            }

            var (scrumObjectType, scrumObjectState) = _timeLineEventObjectParser.ParseEventTypeEnum(elem.EventType);
            var (objectId, objectName) = _timeLineEventObjectParser.ParseScrumObject(scrumObjectType, elem.Data.Task,
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
            var parsedValuesDiff = _timeLineEventObjectParser.ParseValuesDiff(elem.Data.ValuesDiff);
            if (!string.IsNullOrEmpty(elem.Data.Comment))
            {
                parsedValuesDiff.Add(new KeyValuePair<string, string>("Comment", elem.Data.Comment));
            }

            var (scrumObjectType, scrumObjectState) = _timeLineEventObjectParser.ParseEventTypeEnum(elem.EventType);
            var (objectId, objectName) = _timeLineEventObjectParser.ParseScrumObject(scrumObjectType, elem.Data.Task,
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
