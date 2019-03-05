
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Links
    {
        [JsonProperty("pull_request")]
        public Link PullRequest { get; set; }

        [JsonProperty("comments")]
        public Link Comments { get; set; }

        [JsonProperty("self")]
        public Link Self { get; set; }

        [JsonProperty("html")]
        public Link Html { get; set; }

        [JsonProperty("issue")]
        public Link Issue { get; set; }

        //[JsonProperty("comments")]
        //public Link Link { get; set; }

        [JsonProperty("review_comments")]
        public Link ReviewComments { get; set; }

        [JsonProperty("review_comment")]
        public Link ReviewComment { get; set; }

        [JsonProperty("commits")]
        public Link Commits { get; set; }

        [JsonProperty("statuses")]
        public Link Statuses { get; set; }
    }

}
