using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Plan
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("space")]
        public long? Space { get; set; }

        [JsonProperty("private_repos")]
        public long? PrivateRepos { get; set; }

        [JsonProperty("collaborators")]
        public long? Collaborators { get; set; }
    }
}
