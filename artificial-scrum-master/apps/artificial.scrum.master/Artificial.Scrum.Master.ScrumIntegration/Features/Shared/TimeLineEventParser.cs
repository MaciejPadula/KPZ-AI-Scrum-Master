using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models;
using Artificial.Scrum.Master.ScrumIntegration.Features.Shared.ResponseEnums;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;
using Microsoft.IdentityModel.Tokens;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

internal class TimeLineEventParser : ITimeLineEventParser
{
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
            var (objectId, objectName) = ParseObjectIdAndName(elem.Data.Task, elem.Data.Userstory);

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
            var (objectId, objectName) = ParseObjectIdAndName(elem.Data.Task, elem.Data.Userstory);

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

    private static (ScrumObjectType, ScrumObjectState) ParseEventTypeEnum(string eventType)
    {
        var eventTypeSplit = eventType.Split(".");
        if (eventTypeSplit.Length != 3)
        {
            return (ScrumObjectType.Task, ScrumObjectState.Change);
        }

        var scrumObjectTypeParseSuccess =
            Enum.TryParse<ScrumObjectType>(eventTypeSplit[1], ignoreCase: true, out var scrumObjectType);

        var scrumObjectStateParseSuccess =
            Enum.TryParse<ScrumObjectState>(eventTypeSplit[2], ignoreCase: true, out var scrumObjectState);

        if (!scrumObjectTypeParseSuccess || !scrumObjectStateParseSuccess)
        {
            return (ScrumObjectType.Task, ScrumObjectState.Change);
        }

        return (scrumObjectType, scrumObjectState);
    }

    private static (int Id, string Subject) ParseObjectIdAndName(PbiItem? task, Userstory? userStory)
    {
        if (task is not null)
        {
            if (!string.IsNullOrEmpty(task.Subject))
            {
                return (task.Id, task.Subject);
            }

            if (task.Userstory is not null && !string.IsNullOrEmpty(task.Userstory.Subject))
            {
                return (task.Userstory.Id, task.Userstory.Subject);
            }
        }

        if (userStory is not null && !string.IsNullOrEmpty(userStory.Subject))
        {
            return (userStory.Id, userStory.Subject);
        }

        return (-1, string.Empty);
    }
}
