using System;
using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class StatusEvent
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("target_url")]
        public object TargetUrl { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        /// <summary>
        /// The new state. Can be pending, success, failure, or error.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("commit")]
        public Commit Commit { get; set; }

        [JsonProperty("branches")]
        public Branch[] Branches { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }


    public partial class StatusEvent
    {
        public static StatusEvent FromJson(string json) => JsonConvert.DeserializeObject<StatusEvent>(json, Converter.Settings);

        public const string EventString = "status";
    }

}
