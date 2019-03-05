
using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class DeploymentEvent
    {
        [JsonProperty("deployment")]
        public Deployment Deployment { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }

    }

    public partial class DeploymentEvent
    {
        public static DeploymentEvent FromJson(string json) => JsonConvert.DeserializeObject<DeploymentEvent>(json, Converter.Settings);

        public const string EventString = "deployment";
    }
}
