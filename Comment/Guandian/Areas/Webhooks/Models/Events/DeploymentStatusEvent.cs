
using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class DeploymentStatusEvent
    {
        [JsonProperty("deployment_status")]
        public DeploymentStatus DeploymentStatus { get; set; }

        [JsonProperty("deployment")]
        public Deployment Deployment { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
        
    }

    public partial class DeploymentStatusEvent
    {
        public static DeploymentStatusEvent FromJson(string json) => JsonConvert.DeserializeObject<DeploymentStatusEvent>(json, Converter.Settings);

        public const string EventString = "deployment_status";
    }
}
