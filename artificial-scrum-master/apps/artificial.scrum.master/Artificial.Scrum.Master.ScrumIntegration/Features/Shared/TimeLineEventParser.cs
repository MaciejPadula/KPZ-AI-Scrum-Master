using Artificial.Scrum.Master.ScrumIntegration.Features.Project;
using Artificial.Scrum.Master.ScrumIntegration.Features.Timeline;
using Microsoft.IdentityModel.Tokens;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared;

internal class TimeLineEventParser : ITimeLineEventParser
{
    private static readonly Dictionary<string, string> DiffTypeMessages = new()
    {
        ["Comment"] = "has added a comment",
        [$"{nameof(ValuesDiff.AssignedTo)}"] = $"has updated the attribute {nameof(ValuesDiff.AssignedTo)}",
        [$"{nameof(ValuesDiff.Tags)}"] = $"has updated the attribute {nameof(ValuesDiff.Tags)}",
        [$"{nameof(ValuesDiff.Subject)}"] = $"has updated the attribute {nameof(ValuesDiff.Subject)}",
        [$"{nameof(ValuesDiff.Status)}"] = $"has updated the attribute {nameof(ValuesDiff.Status)}",
        [$"{nameof(ValuesDiff.Attachments)}"] = $"has updated the attribute {nameof(ValuesDiff.Attachments)}",
    };

    public GetProfileTimeLineResponse ParseProfileTimeLineElement(List<ProfileTimeLineElementRoot> elements)
    {
        var timelineEvents = elements.Select(elem =>
        {
            var valuesDiff = ParseTaskChangeValuesDiff(elem.Data.ValuesDiff);

            if (!string.IsNullOrEmpty(elem.Data.Comment))
            {
                valuesDiff.Add(new KeyValuePair<string, string>("Comment", elem.Data.Comment));
            }

            return new GetProfileTimeLineResponseElement
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

    public GetProjectTimeLineResponse ParseProjectTimeLineElement(List<ProjectTimeLineElementRoot> elements)
    {
        var timelineEvents = elements.Select(elem =>
        {
            var valuesDiff = ParseTaskChangeValuesDiff(elem.Data.ValuesDiff);

            if (!string.IsNullOrEmpty(elem.Data.Comment))
            {
                valuesDiff.Add(new KeyValuePair<string, string>("Comment", elem.Data.Comment));
            }

            return new GetProjectTimeLineResponseElement
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

    private List<KeyValuePair<string, string>> ParseTaskChangeValuesDiff(ValuesDiff valuesDiff)
    {
        List<KeyValuePair<string, string>> keyValuePairs = new();

        if (!valuesDiff.AssignedTo.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.AssignedTo)}",
                $"from {valuesDiff.AssignedTo?[0]} to {valuesDiff.AssignedTo?[1]}"));
        }

        if (!valuesDiff.Status.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.Status)}",
                $"from {valuesDiff.Status?[0]} to {valuesDiff.Status?[1]}"));
        }

        if (!valuesDiff.Tags.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.Tags)}",
                $"from {valuesDiff.Tags?[0].FirstOrDefault()} to {valuesDiff.Tags?[1].FirstOrDefault()}"));
        }

        if (!valuesDiff.Subject.IsNullOrEmpty())
        {
            keyValuePairs.Add(new KeyValuePair<string, string>(
                $"{nameof(ValuesDiff.Subject)}",
                $"from {valuesDiff.Subject?[0]} to {valuesDiff.Subject?[1]}"));
        }

        if (valuesDiff.Attachments is not null)
        {
            if (!valuesDiff.Attachments.New.IsNullOrEmpty())
            {
                keyValuePairs.Add(new KeyValuePair<string, string>(
                    $"{nameof(ValuesDiff.Attachments)}",
                    valuesDiff.Attachments.New.FirstOrDefault()?.Url));
            }
        }

        return keyValuePairs;
    }

    private List<KeyValuePair<string, string>> ParseUserStoryChangeValuesDiff(ValuesDiff valuesDiff, User dataUser)
    {
        List<KeyValuePair<string, string>> values = new();

        return values;


        return valuesDiff.GetType().GetProperties()
            .Select(prop =>
            {
                var key = prop.Name;
                var value = prop.GetValue(valuesDiff);
                return new KeyValuePair<string, string>(key, DiffTypeMessages[key] + " to " + value);
            }).ToList();
    }
}
