using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;
using Microsoft.IdentityModel.Tokens;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

internal class TimeLineEventParser : ITimeLineEventParser
{
    public GetProfileTimeLineResponse ParseProfileTimeLineElement(IEnumerable<ProfileTimeLineElementRoot> elements)
    {
        var timelineEvents = elements.Select(elem =>
        {
            var valuesDiff = ParseValuesDiff(elem.Data.ValuesDiff);
            if (!string.IsNullOrEmpty(elem.Data.Comment))
            {
                valuesDiff.Add(new KeyValuePair<string, string>("Comment", elem.Data.Comment));
            }

            return new GetProfileTimeLineResponseEvent
            {
                Id = elem.Id,
                EventType = elem.EventType,
                Created = elem.Created,
                ProjectId = elem.Project,
                TaskId = elem.Data.Task?.Id ?? -1,
                TaskSubject = elem.Data.Task?.Subject,
                TaskUserStoryId = elem.Data.Task?.Userstory?.Id ?? -1,
                TaskUserStorySubject = elem.Data.Task?.Userstory?.Subject,
                UserId = elem.Data.User.Id,
                UserName = elem.Data.User.Name,
                UserPhoto = elem.Data.User.Photo,
                UserUsername = elem.Data.User.Username,
                ProjectName = elem.Data.Project.Name,
                MileStoneId = elem.Data.Milestone?.Id ?? -1,
                MileStoneName = elem.Data.Milestone?.Name,
                UserStoryId = elem.Data.Userstory?.Id ?? -1,
                UserStorySubject = elem.Data.Userstory?.Subject,
                ValuesDiff = valuesDiff
            };
        }).ToList();

        return new GetProfileTimeLineResponse { TimeLineEvents = timelineEvents };
    }

    public GetProjectTimeLineResponse ParseProjectTimeLineElement(IEnumerable<ProjectTimeLineElementRoot> elements)
    {
        var timelineEvents = elements.Select(elem =>
        {
            var valuesDiff = ParseValuesDiff(elem.Data.ValuesDiff);
            if (!string.IsNullOrEmpty(elem.Data.Comment))
            {
                valuesDiff.Add(new KeyValuePair<string, string>("Comment", elem.Data.Comment));
            }

            return new GetProjectTimeLineResponseEvent
            {
                Id = elem.Id,
                EventType = elem.EventType,
                Created = elem.Created,
                ProjectId = elem.Project,
                TaskId = elem.Data.Task?.Id ?? -1,
                TaskSubject = elem.Data.Task?.Subject,
                TaskUserStoryId = elem.Data.Task?.Userstory?.Id ?? -1,
                TaskUserStorySubject = elem.Data.Task?.Userstory?.Subject,
                UserId = elem.Data.User.Id,
                UserName = elem.Data.User.Name,
                UserPhoto = elem.Data.User.Photo,
                UserUsername = elem.Data.User.Username,
                ProjectName = elem.Data.Project.Name,
                ValuesDiff = valuesDiff
            };
        }).ToList();

        return new GetProjectTimeLineResponse { TimeLineEvents = timelineEvents };
    }

    private static List<KeyValuePair<string, string>> ParseValuesDiff(ValuesDiff valuesDiff)
    {
        List<KeyValuePair<string, string>> keyValuePairs = [];

        if (!valuesDiff.AssignedTo.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.AssignedTo)}",
                $"from {valuesDiff.AssignedTo?[0] ?? "None"} to {valuesDiff.AssignedTo?[1] ?? "None"}"));
        }

        if (!valuesDiff.Status.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.Status)}",
                $"from {valuesDiff.Status?[0] ?? "None"} to {valuesDiff.Status?[1] ?? "None"}"));
        }

        if (!valuesDiff.Tags.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.Tags)}",
                $"from {valuesDiff.Tags?[0].FirstOrDefault() ?? "None"} to {valuesDiff.Tags?[1].FirstOrDefault() ?? "None"}"));
        }

        if (!valuesDiff.Subject.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.Subject)}",
                $"from {valuesDiff.Subject?[0] ?? "None"} to {valuesDiff.Subject?[1] ?? "None"}"));
        }

        if (!string.IsNullOrEmpty(valuesDiff.DescriptionDiff))
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.DescriptionDiff)}", "Description has been changed"));
        }

        if (valuesDiff.Attachments is not null && !valuesDiff.Attachments.New.IsNullOrEmpty())
        {
            var value = valuesDiff.Attachments.New?.FirstOrDefault()?.Url;
            if (value is not null)
            {
                keyValuePairs.Add(new KeyValuePair<string, string>(
                    $"{nameof(ValuesDiff.Attachments)}", value));
            }
        }

        return keyValuePairs;
    }
}

// TODO : Event type na Enum!!!
