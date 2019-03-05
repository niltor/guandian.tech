using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Link
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}
