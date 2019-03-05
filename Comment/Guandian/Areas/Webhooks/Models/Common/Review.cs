
using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Review
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("submitted_at")]
        public DateTime? SubmittedAt { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("pull_request_url")]
        public string PullRequestUrl { get; set; }

        [JsonProperty("_links")]
        public Links Links { get; set; }
    }

}
