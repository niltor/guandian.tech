using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Deployment
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("task")]
        public string Task { get; set; }

        [JsonProperty("payload")]
        public Payload Payload { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("creator")]
        public User Creator { get; set; }

        [JsonProperty("created_at")]

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("statuses_url")]
        public string StatusesUrl { get; set; }

        [JsonProperty("repository_url")]
        public string RepositoryUrl { get; set; }
    }
}
