using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class PullRequestEvent
    {
        /// <summary>
        /// Can be one of "assigned", "unassigned", "review_requested", "review_request_removed", "labeled", "unlabeled", "opened", "edited", "closed", or "reopened". 
        /// If the action is "closed" and the merged key is false, the pull request was closed with unmerged commits.
        /// If the action is "closed" and the merged key is true, the pull request was merged.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("number")]
        public long? Number { get; set; }

        //TODO: [JsonProperty("changes")]

        [JsonProperty("pull_request")]
        public PullRequest PullRequest { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }

        [JsonProperty("installation")]
        public Installation Installation { get; set; }
    }


    public partial class PullRequestEvent
    {
        public static PullRequestEvent FromJson(string json) => JsonConvert.DeserializeObject<PullRequestEvent>(json, Converter.Settings);

        public const string EventString = "pull_request";
    }
    
}
