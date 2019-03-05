using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class CommitCommentEvent
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("comment")]
        public Comment Comment { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class CommitCommentEvent
    {
        public static CommitCommentEvent FromJson(string json) => JsonConvert.DeserializeObject<CommitCommentEvent>(json, Converter.Settings);

        public const string EventString = "commit_comment";
        
    }


}
