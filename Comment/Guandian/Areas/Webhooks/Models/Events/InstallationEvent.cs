using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class InstallationEvent
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("installation")]
        public Installation Installation { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class InstallationEvent
    {
        public static InstallationEvent FromJson(string json) => JsonConvert.DeserializeObject<InstallationEvent>(json, Converter.Settings);

        public const string EventString = "installation";
    }
}
