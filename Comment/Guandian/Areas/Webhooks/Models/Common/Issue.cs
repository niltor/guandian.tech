
using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Issue
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("repository_url")]
        public string RepositoryUrl { get; set; }

        [JsonProperty("labels_url")]
        public string LabelsUrl { get; set; }

        [JsonProperty("comments_url")]
        public string CommentsUrl { get; set; }

        [JsonProperty("events_url")]
        public string EventsUrl { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("number")]
        public long? Number { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("labels")]
        public Label[] Labels { get; set; }

        [JsonProperty("assignee")]
        public User Assignee { get; set; }

        [JsonProperty("assignees")]
        public User[] Assignees { get; set; }

        [JsonProperty("milestone")]
        public Milestone Milestone { get; set; }

        [JsonProperty("locked")]
        public bool? Locked { get; set; }

        [JsonProperty("active_lock_reason")]
        public string ActiveLockReason { get; set; }

        [JsonProperty("comments")]
        public long? Comments { get; set; }

        [JsonProperty("pull_request")]
        public PullRequest PullRequest { get; set; }

        [JsonProperty("closed_at")]
        public object ClosedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]        
        public DateTime? CreatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("updated_at")]        
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("closed_by")]
        public User ClosedBy { get; set; }
    }
}
