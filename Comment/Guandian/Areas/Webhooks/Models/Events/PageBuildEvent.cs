using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class PageBuildEvent
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("build")]
        public Build Build { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class PageBuildEvent
    {
        public static PageBuildEvent FromJson(string json) => JsonConvert.DeserializeObject<PageBuildEvent>(json, Converter.Settings);
        public const string EventString = "page_build";
    }
    
}
