using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class MilestoneEvent
    {
        /// <summary>
        /// Can be one of "created", "closed", "opened", "edited", or "deleted".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("milestone")]
        public Milestone Milestone { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class MilestoneEvent
    {
        public static MilestoneEvent FromJson(string json) => JsonConvert.DeserializeObject<MilestoneEvent>(json, Converter.Settings);

        public const string EventString = "milestone";
    }
}
