
using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class GollumEvent
    {
        [JsonProperty("pages")]
        public Page[] Pages { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class GollumEvent
    {
        public static GollumEvent FromJson(string json) => JsonConvert.DeserializeObject<GollumEvent>(json, Converter.Settings);

        public const string EventString = "gollum";
    }
}
