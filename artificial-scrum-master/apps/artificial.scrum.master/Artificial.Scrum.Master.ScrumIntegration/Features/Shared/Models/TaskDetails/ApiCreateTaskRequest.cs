using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.ScrumIntegration.Features.Shared.Models.TaskDetails;
internal class ApiCreateTaskRequest
{
    //[JsonPropertyName("assigned_to")]
    public int? assigned_to { get; set; }

    //[JsonPropertyName("blocked_note")]
    public string? blocked_note { get; set; }

    //[JsonPropertyName("description")]
    public string? description { get; set; }

    //[JsonPropertyName("external_reference")]
    public string? external_reference { get; set; }

    //[JsonPropertyName("is_blocked")]
    public bool? is_blocked { get; set; }

    //[JsonPropertyName("is_closed")]
    public bool? is_closed { get; set; }

    //[JsonPropertyName("is_iocaine")]
    public bool? is_iocaine { get; set; }

    //[JsonPropertyName("milestone")]
    public int? milestone { get; set; }

    //[JsonPropertyName("project")]
    public int? project { get; set; }

    //[JsonPropertyName("status")]
    public int? status { get; set; }

    //[JsonPropertyName("subject")]
    public string? subject { get; set; }

    //[JsonPropertyName("tags")]
    public List<string> tags { get; set; } = new List<string>();

    //[JsonPropertyName("taskboard_order")]
    public int? taskboard_order { get; set; }

    //[JsonPropertyName("us_order")]
    public int? us_order { get; set; }

    //[JsonPropertyName("user_story")]
    public int? user_story { get; set; }

    //[JsonPropertyName("watchers")]/
    public List<int> watchers { get; set; } = new List<int>();
}
