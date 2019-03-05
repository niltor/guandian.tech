using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Stats
    {
        [JsonProperty("additions")]
        public long? Additions { get; set; }

        [JsonProperty("deletions")]
        public long? Deletions { get; set; }

        [JsonProperty("total")]
        public long? Total { get; set; }
    }
}
