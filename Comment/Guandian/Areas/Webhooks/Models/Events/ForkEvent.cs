using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class ForkEvent
    {
        [JsonProperty("forkee")]
        public Repository Forkee { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }

    }

    public partial class ForkEvent
    {
        public static ForkEvent FromJson(string json) => JsonConvert.DeserializeObject<ForkEvent>(json, Converter.Settings);

        public const string EventString = "fork";
    }
}
