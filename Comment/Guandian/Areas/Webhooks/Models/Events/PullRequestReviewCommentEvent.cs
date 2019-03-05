using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class PullRequestReviewCommentEvent
    {
        /// <summary>
        ///  Can be one of "created", "edited", or "deleted".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        //TODO:[JsonProperty("changes")]

        [JsonProperty("comment")]
        public Comment Comment { get; set; }

        [JsonProperty("pull_request")]
        public PullRequest PullRequest { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }


    public partial class PullRequestReviewCommentEvent
    {
        public static PullRequestReviewCommentEvent FromJson(string json) => JsonConvert.DeserializeObject<PullRequestReviewCommentEvent>(json, Converter.Settings);

        public const string EventString = "pull_request_review_comment";
    }
    
}
