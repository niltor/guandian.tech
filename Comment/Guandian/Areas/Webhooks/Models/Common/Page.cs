using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Page
    {
        [JsonProperty("page_name")]
        public string PageName { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("summary")]
        public object Summary { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }
}
