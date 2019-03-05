using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Permissions
    {
        [JsonProperty("admin")]
        public bool? Admin { get; set; }

        [JsonProperty("push")]
        public bool? Push { get; set; }

        [JsonProperty("pull")]
        public bool? Pull { get; set; }
    }

}
