using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.ResponseEnums;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

internal class TimeLineEventParser : ITimeLineEventParser
{
    private readonly ILogger<TimeLineEventParser> _logger;

    public TimeLineEventParser(ILogger<TimeLineEventParser> logger)
    {
        _logger = logger;
    }

    public GetProfileTimeLineResponse ParseProfileTimeLineElement(IEnumerable<TimeLineEventRoot> elements)
    {
        var timelineEvents = elements.Select(elem =>
        {
            var parsedValuesDiff = ParseValuesDiff(elem.Data.ValuesDiff);
            if (!string.IsNullOrEmpty(elem.Data.Comment))
            {
                parsedValuesDiff.Add(new KeyValuePair<string, string>("Comment", elem.Data.Comment));
            }

            var (scrumObjectType, scrumObjectState) = ParseEventTypeEnum(elem.EventType);
            var (objectId, objectName) = ParseScrumObject(scrumObjectType, elem.Data.Task,
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
            var parsedValuesDiff = ParseValuesDiff(elem.Data.ValuesDiff);
            if (!string.IsNullOrEmpty(elem.Data.Comment))
            {
                parsedValuesDiff.Add(new KeyValuePair<string, string>("Comment", elem.Data.Comment));
            }

            var (scrumObjectType, scrumObjectState) = ParseEventTypeEnum(elem.EventType);
            var (objectId, objectName) = ParseScrumObject(scrumObjectType, elem.Data.Task,
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

    private (ScrumObjectType, ScrumObjectState) ParseEventTypeEnum(string eventType)
    {
        var eventTypeSplit = eventType.Split(".");
        if (eventTypeSplit.Length != 3)
        {
            _logger.LogError("Event type:{EventType} is not in the correct format", eventType);
        }

        var scrumObjectType = eventTypeSplit[1] switch
        {
            "task" => ScrumObjectType.Task,
            "userstory" => ScrumObjectType.UserStory,
            "membership" => ScrumObjectType.Membership,
            "milestone" => ScrumObjectType.Sprint,
            "project" => ScrumObjectType.Project,
            _ => ScrumObjectType.None
        };

        var scrumObjectState = eventTypeSplit[2] switch
        {
            "create" => ScrumObjectState.Create,
            "change" => ScrumObjectState.Change,
            "delete" => ScrumObjectState.Delete,
            _ => ScrumObjectState.None
        };

        if (scrumObjectType == ScrumObjectType.None || scrumObjectState == ScrumObjectState.None)
        {
            _logger.LogError("Event type:{EventType} unknown", eventType);
        }

        return (scrumObjectType, scrumObjectState);
    }

    private (int Id, string Subject) ParseScrumObject(
        ScrumObjectType scrumObjectType, PbiItem? task, Userstory? userStory,
        Milestone? milestone, User user, Models.Project? project)
    {
        return scrumObjectType switch
        {
            ScrumObjectType.Task when task is not null =>
                ParseTaskObject(task),

            ScrumObjectType.UserStory when userStory is not null && !string.IsNullOrEmpty(userStory.Subject) =>
                (userStory.Id, userStory.Subject),

            ScrumObjectType.Sprint when milestone is not null =>
                (milestone.Id, milestone.Name),

            ScrumObjectType.Membership =>
                (user.Id, user.Name),

            ScrumObjectType.Project when project is not null =>
                (project.Id, project.Name),

            _ =>
                HandleScrumObjectParsingFailure(scrumObjectType)
        };
    }

    private (int, string) ParseTaskObject(PbiItem task)
    {
        if (!string.IsNullOrEmpty(task.Subject))
            return (task.Id, task.Subject);

        if (task.Userstory is not null && !string.IsNullOrEmpty(task.Userstory.Subject))
            return (task.Userstory.Id, task.Userstory.Subject);

        return HandleScrumObjectParsingFailure(ScrumObjectType.Task);
    }

    private (int, string) HandleScrumObjectParsingFailure(ScrumObjectType scrumObjectType)
    {
        _logger.LogError("ScrumObjectType:{ScrumObjectType} is not supported", scrumObjectType.ToString());
        return (-1, string.Empty);
    }

    private static List<KeyValuePair<string, string>> ParseValuesDiff(ValuesDiff? valuesDiff)
    {
        List<KeyValuePair<string, string>> keyValuePairs = [];
        if (valuesDiff is null)
        {
            return keyValuePairs;
        }

        if (!valuesDiff.AssignedTo.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.AssignedTo)}",
                $"from '{valuesDiff.AssignedTo?[0] ?? "None"}' to '{valuesDiff.AssignedTo?[1] ?? "None"}'"));
        }

        if (!valuesDiff.Status.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.Status)}",
                $"from '{valuesDiff.Status?[0] ?? "None"}' to '{valuesDiff.Status?[1] ?? "None"}'"));
        }

        if (!valuesDiff.Tags.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.Tags)}",
                $"from '{valuesDiff.Tags?[0].FirstOrDefault() ?? "None"}' to '{valuesDiff.Tags?[1].FirstOrDefault() ?? "None"}'"));
        }

        if (!valuesDiff.Subject.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.Subject)}",
                $"from '{valuesDiff.Subject?[0] ?? "None"}' to '{valuesDiff.Subject?[1] ?? "None"}'"));
        }

        if (!string.IsNullOrEmpty(valuesDiff.DescriptionDiff))
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.DescriptionDiff)}", "Description has been changed"));
        }

        if (valuesDiff.Attachments is null || valuesDiff.Attachments.New.IsNullOrEmpty()) return keyValuePairs;

        var value = valuesDiff.Attachments.New?.FirstOrDefault()?.Url;
        if (value is not null)
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.Attachments)}", value));
        }

        return keyValuePairs;
    }
}
