
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Installation
    {
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("account")]
        public User Account { get; set; }

        [JsonProperty("repository_selection")]
        public string RepositorySelection { get; set; }

        [JsonProperty("access_tokens_url")]
        public string AccessTokensUrl { get; set; }

        [JsonProperty("repositories_url")]
        public string RepositoriesUrl { get; set; }
    }
}
