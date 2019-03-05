using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class ProjectCardEvent
    {
        /// <summary>
        /// Can be "created", "edited", "converted", "moved", or "deleted".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        //TODO: [JsonProperty("changes")]

        [JsonProperty("after_id")]
        public long? AfterId { get; set; }

        [JsonProperty("project_card")]
        public ProjectCard ProjectCard { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class ProjectCardEvent
    {
        public static ProjectCardEvent FromJson(string json) => JsonConvert.DeserializeObject<ProjectCardEvent>(json, Converter.Settings);

        public const string EventString = "project_card";
    }
 
}
