using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class User
    {
        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("date")]        
        public DateTime? Date { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

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

        [JsonProperty("hireable")]
        public bool? Hireable { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("public_repos")]
        public long? PublicRepos { get; set; }

        [JsonProperty("public_gists")]
        public long? PublicGists { get; set; }

        [JsonProperty("followers")]
        public long? Followers { get; set; }

        [JsonProperty("following")]
        public long? Following { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]        
        public DateTime? CreatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("updated_at")]        
        public DateTime? UpdatedAt { get; set; }

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

        [JsonProperty("two_factor_authentication")]
        public bool? TwoFactorAuthentication { get; set; }

        [JsonProperty("plan")]
        public Plan Plan { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("gravatar_id")]
        public string GravatarId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("followers_url")]
        public string FollowersUrl { get; set; }

        [JsonProperty("following_url")]
        public string FollowingUrl { get; set; }

        [JsonProperty("gists_url")]
        public string GistsUrl { get; set; }

        [JsonProperty("starred_url")]
        public string StarredUrl { get; set; }

        [JsonProperty("subscriptions_url")]
        public string SubscriptionsUrl { get; set; }

        [JsonProperty("organizations_url")]
        public string OrganizationsUrl { get; set; }

        [JsonProperty("repos_url")]
        public string ReposUrl { get; set; }

        [JsonProperty("events_url")]
        public string EventsUrl { get; set; }

        [JsonProperty("received_events_url")]
        public string ReceivedEventsUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("site_admin")]
        public bool? SiteAdmin { get; set; }
    }

}
