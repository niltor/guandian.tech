using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class PushEvent
    {
        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("head")]
        public string Head { get; set; }

        [JsonProperty("before")]
        public string Before { get; set; }
        
        [JsonProperty("after")]
        public string After { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("distinct_size")]
        public int DistinctSize { get; set; }

        [JsonProperty("created")]
        public bool? Created { get; set; }

        [JsonProperty("deleted")]
        public bool? Deleted { get; set; }

        [JsonProperty("forced")]
        public bool? Forced { get; set; }

        [JsonProperty("base_ref")]
        public object BaseRef { get; set; }

        [JsonProperty("compare")]
        public string Compare { get; set; }

        [JsonProperty("commits")]
        public Commit[] Commits { get; set; }

        [JsonProperty("head_commit")]
        public Commit HeadCommit { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("pusher")]
        public User Pusher { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }


    public partial class PushEvent
    {
        public static PushEvent FromJson(string json) => JsonConvert.DeserializeObject<PushEvent>(json, Converter.Settings);

        public const string EventString = "push";
    }
}
