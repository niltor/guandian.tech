using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Organization
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("blog")]
        public string Blog { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("public_repos")]
        public long? PublicRepos { get; set; }

        [JsonProperty("public_gists")]
        public long? PublicGists { get; set; }

        [JsonProperty("followers")]
        public long? Followers { get; set; }

        [JsonProperty("following")]
        public long? Following { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]        
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("total_private_repos")]
        public long? TotalPrivateRepos { get; set; }

        [JsonProperty("owned_private_repos")]
        public long? OwnedPrivateRepos { get; set; }

        [JsonProperty("private_gists")]
        public long? PrivateGists { get; set; }

        [JsonProperty("disk_usage")]
        public long? DiskUsage { get; set; }

        [JsonProperty("collaborators")]
        public long? Collaborators { get; set; }

        [JsonProperty("billing_email")]
        public string BillingEmail { get; set; }

        [JsonProperty("plan")]
        public Plan Plan { get; set; }

        [JsonProperty("default_repository_settings")]
        public string DefaultRepositorySettings { get; set; }

        [JsonProperty("members_can_create_repositories")]
        public bool? MembersCanCreateRepositories { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("repos_url")]
        public string ReposUrl { get; set; }

        [JsonProperty("events_url")]
        public string EventsUrl { get; set; }

        [JsonProperty("hooks_url")]
        public string HooksUrl { get; set; }

        [JsonProperty("issues_url")]
        public string IssuesUrl { get; set; }

        [JsonProperty("members_url")]
        public string MembersUrl { get; set; }

        [JsonProperty("public_members_url")]
        public string PublicMembersUrl { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
