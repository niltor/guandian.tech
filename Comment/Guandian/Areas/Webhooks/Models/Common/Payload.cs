using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Payload
    {
        [JsonProperty("task")]
        public string Task { get; set; }
    }
}
