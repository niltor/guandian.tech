using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Branch
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("commit")]
        public Commit Commit { get; set; }
    }

}
