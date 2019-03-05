// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using GithubWebhook;
//
//    var data = ProjectEvent.FromJson(jsonString);

using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class ProjectEvent
    {
        /// <summary>
        /// Can be one of "created", "edited", "closed", "reopened", or "deleted".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        //TODO: [JsonProperty("changes")]

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }


    public partial class ProjectEvent
    {
        public static ProjectEvent FromJson(string json) => JsonConvert.DeserializeObject<ProjectEvent>(json, Converter.Settings);

        public const string EventString = "project";
    }

}
