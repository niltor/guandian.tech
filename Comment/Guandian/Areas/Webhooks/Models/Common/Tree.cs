using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Tree
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("sha")]
        public string Sha { get; set; }
    }
}
