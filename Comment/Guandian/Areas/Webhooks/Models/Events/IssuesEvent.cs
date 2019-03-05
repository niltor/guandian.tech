using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class IssuesEvent
    {
        /// <summary>
        /// Can be "assigned", "unassigned", "labeled", "unlabeled", "opened", "edited", "milestoned", "demilestoned", "closed", or "reopened"
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("issue")]
        public Issue Issue { get; set; }

        //TODO: [JsonProperty("changes")] (Have not found documentation)

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class IssuesEvent
    {
        public static IssuesEvent FromJson(string json) => JsonConvert.DeserializeObject<IssuesEvent>(json, Converter.Settings);

        public const string EventString = "issues";
        
    }
}
