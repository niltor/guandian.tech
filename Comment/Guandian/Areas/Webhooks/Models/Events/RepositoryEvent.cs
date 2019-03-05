// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using GithubWebhook;
//
//    var data = RepositoryEvent.FromJson(jsonString);

using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class RepositoryEvent
    {
        /// <summary>
        /// This can be one of created, deleted (organization hooks only), archived, unarchived, publicized, or privatized.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class RepositoryEvent
    {
        public static RepositoryEvent FromJson(string json) => JsonConvert.DeserializeObject<RepositoryEvent>(json, Converter.Settings);

        public const string EventString = "repository";
    }

}
