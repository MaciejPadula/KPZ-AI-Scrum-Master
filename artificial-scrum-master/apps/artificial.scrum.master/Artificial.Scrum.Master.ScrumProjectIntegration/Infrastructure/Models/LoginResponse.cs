using System.Text.Json.Serialization;

namespace Artificial.Scrum.Master.ScrumProjectIntegration.Infrastructure.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("username")] public string Username { get; set; } = default!;
        [JsonPropertyName("full_name")] public string FullName { get; set; } = default!;

        [JsonPropertyName("full_name_display")]
        public string FullNameDisplay { get; set; } = default!;

        [JsonPropertyName("color")] public string Color { get; set; } = default!;
        [JsonPropertyName("bio")] public string Bio { get; set; } = default!;
        [JsonPropertyName("lang")] public string Lang { get; set; } = default!;
        [JsonPropertyName("theme")] public string Theme { get; set; } = default!;
        [JsonPropertyName("timezone")] public string Timezone { get; set; } = default!;
        [JsonPropertyName("is_active")] public bool IsActive { get; set; }
        [JsonPropertyName("photo")] public object Photo { get; set; } = default!;
        [JsonPropertyName("big_photo")] public object BigPhoto { get; set; } = default!;
        [JsonPropertyName("gravatar_id")] public string GravatarId { get; set; } = default!;
        [JsonPropertyName("roles")] public List<string> Roles { get; set; } = default!;

        [JsonPropertyName("total_private_projects")]
        public int TotalPrivateProjects { get; set; }

        [JsonPropertyName("total_public_projects")]
        public int TotalPublicProjects { get; set; }

        [JsonPropertyName("email")] public string Email { get; set; } = default!;
        [JsonPropertyName("uuid")] public string Uuid { get; set; } = default!;
        [JsonPropertyName("date_joined")] public DateTime DateJoined { get; set; }
        [JsonPropertyName("read_new_terms")] public bool ReadNewTerms { get; set; }
        [JsonPropertyName("accepted_terms")] public bool AcceptedTerms { get; set; }

        [JsonPropertyName("max_private_projects")]
        public object MaxPrivateProjects { get; set; } = default!;

        [JsonPropertyName("max_public_projects")]
        public object MaxPublicProjects { get; set; } = default!;

        [JsonPropertyName("max_memberships_private_projects")]
        public object MaxMembershipsPrivateProjects { get; set; } = default!;

        [JsonPropertyName("max_memberships_public_projects")]
        public object MaxMembershipsPublicProjects { get; set; } = default!;

        [JsonPropertyName("verified_email")] public bool VerifiedEmail { get; set; }


        [JsonPropertyName("refresh")] public string RefreshToken { get; set; } = default!;
        [JsonPropertyName("auth_token")] public string AccessToken { get; set; } = default!;
    }
}
