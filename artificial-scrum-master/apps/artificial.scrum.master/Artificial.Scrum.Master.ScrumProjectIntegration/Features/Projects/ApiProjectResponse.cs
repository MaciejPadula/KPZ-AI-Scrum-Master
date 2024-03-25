using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Features.Projects
{
    public class Project
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; } = default!;
        [JsonPropertyName("slug")] public string Slug { get; set; } = default!;
        [JsonPropertyName("description")] public string Description { get; set; } = default!;

        [JsonPropertyName("created_date")] public DateTime CreatedDate { get; set; }

        [JsonPropertyName("modified_date")] public DateTime ModifiedDate { get; set; }

        [JsonPropertyName("owner")] public Owner Owner { get; set; } = default!;

        [JsonPropertyName("members")] public List<int> Members { get; set; } = default!;

        [JsonPropertyName("total_milestones")] public object TotalMilestones { get; set; } = default!;

        [JsonPropertyName("total_story_points")]
        public object TotalStoryPoints { get; set; } = default!;

        [JsonPropertyName("is_contact_activated")]
        public bool IsContactActivated { get; set; }

        [JsonPropertyName("is_epics_activated")]
        public bool IsEpicsActivated { get; set; }

        [JsonPropertyName("is_backlog_activated")]
        public bool IsBacklogActivated { get; set; }

        [JsonPropertyName("is_kanban_activated")]
        public bool IsKanbanActivated { get; set; }

        [JsonPropertyName("is_wiki_activated")]
        public bool IsWikiActivated { get; set; }

        [JsonPropertyName("is_issues_activated")]
        public bool IsIssuesActivated { get; set; }

        [JsonPropertyName("videoconferences")] public object Videoconferences { get; set; } = default!;

        [JsonPropertyName("videoconferences_extra_data")]
        public object VideoconferencesExtraData { get; set; } = default!;

        [JsonPropertyName("creation_template")]
        public int CreationTemplate { get; set; }

        [JsonPropertyName("is_private")] public bool IsPrivate { get; set; }

        [JsonPropertyName("anon_permissions")] public List<string> AnonPermissions { get; set; } = default!;

        [JsonPropertyName("public_permissions")]
        public List<string> PublicPermissions { get; set; } = default!;

        [JsonPropertyName("is_featured")] public bool IsFeatured { get; set; }

        [JsonPropertyName("is_looking_for_people")]
        public bool IsLookingForPeople { get; set; }

        [JsonPropertyName("looking_for_people_note")]
        public string LookingForPeopleNote { get; set; } = default!;

        [JsonPropertyName("blocked_code")] public object BlockedCode { get; set; } = default!;

        [JsonPropertyName("totals_updated_datetime")]
        public DateTime TotalsUpdatedDatetime { get; set; }

        [JsonPropertyName("total_fans")] public int TotalFans { get; set; }

        [JsonPropertyName("total_fans_last_week")]
        public int TotalFansLastWeek { get; set; }

        [JsonPropertyName("total_fans_last_month")]
        public int TotalFansLastMonth { get; set; }

        [JsonPropertyName("total_fans_last_year")]
        public int TotalFansLastYear { get; set; }

        [JsonPropertyName("total_activity")] public int TotalActivity { get; set; }

        [JsonPropertyName("total_activity_last_week")]
        public int TotalActivityLastWeek { get; set; }

        [JsonPropertyName("total_activity_last_month")]
        public int TotalActivityLastMonth { get; set; }

        [JsonPropertyName("total_activity_last_year")]
        public int TotalActivityLastYear { get; set; }

        [JsonPropertyName("tags")] public List<object> Tags { get; set; } = default!;

        [JsonPropertyName("tags_colors")] public TagsColors TagsColors { get; set; } = default!;

        [JsonPropertyName("default_epic_status")]
        public int DefaultEpicStatus { get; set; }

        [JsonPropertyName("default_points")] public int DefaultPoints { get; set; }

        [JsonPropertyName("default_us_status")]
        public int DefaultUsStatus { get; set; }

        [JsonPropertyName("default_task_status")]
        public int DefaultTaskStatus { get; set; }

        [JsonPropertyName("default_priority")] public int DefaultPriority { get; set; }

        [JsonPropertyName("default_severity")] public int DefaultSeverity { get; set; }

        [JsonPropertyName("default_issue_status")]

        public int DefaultIssueStatus { get; set; }

        [JsonPropertyName("default_issue_type")]
        public int DefaultIssueType { get; set; }

        [JsonPropertyName("default_swimlane")] public object DefaultSwimLane { get; set; } = default!;

        [JsonPropertyName("my_permissions")] public List<string> MyPermissions { get; set; } = default!;

        [JsonPropertyName("i_am_owner")] public bool AmOwner { get; set; }

        [JsonPropertyName("i_am_admin")] public bool AmAdmin { get; set; }

        [JsonPropertyName("i_am_member")] public bool AmMember { get; set; }

        [JsonPropertyName("notify_level")] public int NotifyLevel { get; set; }

        [JsonPropertyName("total_closed_milestones")]
        public int TotalClosedMilestones { get; set; }

        [JsonPropertyName("is_watcher")] public bool IsWatcher { get; set; }

        [JsonPropertyName("total_watchers")] public int TotalWatchers { get; set; }

        [JsonPropertyName("logo_small_url")] public object LogoSmallUrl { get; set; } = default!;

        [JsonPropertyName("logo_big_url")] public object LogoBigUrl { get; set; } = default!;

        [JsonPropertyName("is_fan")] public bool IsFan { get; set; }

        [JsonPropertyName("my_homepage")] public object MyHomepage { get; set; } = default!;
    }

    public class Owner
    {
        [JsonPropertyName("username")] public string Username { get; set; } = default!;

        [JsonPropertyName("full_name_display")]
        public string FullNameDisplay { get; set; } = default!;

        [JsonPropertyName("photo")] public string Photo { get; set; } = default!;

        [JsonPropertyName("big_photo")] public string BigPhoto { get; set; } = default!;

        [JsonPropertyName("gravatar_id")] public string GravatarId { get; set; } = default!;

        [JsonPropertyName("is_active")] public bool IsActive { get; set; }

        [JsonPropertyName("id")] public int Id { get; set; }
    }

    public class TagsColors
    {
        [JsonPropertyName("cr")] public string Cr { get; set; } = default!;
    }
}
