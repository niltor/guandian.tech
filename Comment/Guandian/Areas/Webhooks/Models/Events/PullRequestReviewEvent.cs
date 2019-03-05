using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class PullRequestReviewEvent
    {
        /// <summary>
        /// Can be "submitted", "edited", or "dismissed".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("review")]
        public Review Review { get; set; }

        [JsonProperty("pull_request")]
        public PullRequest PullRequest { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class PullRequestReviewEvent
    {
        public static PullRequestReviewEvent FromJson(string json) => JsonConvert.DeserializeObject<PullRequestReviewEvent>(json, Converter.Settings);

        public const string EventString = "pull_request_review";
    }

}
