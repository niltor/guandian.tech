using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Milestone
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("labels_url")]
        public string LabelsUrl { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("number")]
        public long? Number { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("creator")]
        public User Creator { get; set; }

        [JsonProperty("open_issues")]
        public long? OpenIssues { get; set; }

        [JsonProperty("closed_issues")]
        public long? ClosedIssues { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]        
        public DateTime? CreatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("updated_at")]        
        public DateTime? UpdatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("closed_at")]        
        public DateTime? ClosedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("due_on")]        
        public DateTime? DueOn { get; set; }
    }
}
