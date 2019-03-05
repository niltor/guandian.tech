using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class ProjectColumnEvent
    {
        /// <summary>
        /// Can be one of "created", "edited", "moved" or "deleted".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        //TODO:  [JsonProperty("changes")]

        [JsonProperty("after_id")]
        public long? AfterId { get; set; }

        [JsonProperty("project_column")]
        public ProjectColumn ProjectColumn { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }
   

    public partial class ProjectColumnEvent
    {
        public static ProjectColumnEvent FromJson(string json) => JsonConvert.DeserializeObject<ProjectColumnEvent>(json, Converter.Settings);

        public const string EventString = "project_column";
    }
   
}
