using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Label
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("default")]
        public bool? Default { get; set; }
    }
}
