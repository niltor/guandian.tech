using System;
using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{ 

    /// <summary>
    /// Not tested!
    /// </summary>
    public partial class PingEvent
    {
        [JsonProperty("zen")]
        public string Zen { get; set; }

        [JsonProperty("hook_id")]
        public long HookId { get; set; }

        [JsonProperty("hook")]
        public Hook Hook { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class Hook
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        [JsonProperty("events")]
        public string[] Events { get; set; }

        [JsonProperty("config")]
        public Config Config { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("test_url")]
        public string TestUrl { get; set; }

        [JsonProperty("ping_url")]
        public string PingUrl { get; set; }

        [JsonProperty("last_response")]
        public LastResponse LastResponse { get; set; }
    }

    public partial class Config
    {
        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("insecure_ssl")]
        public string InsecureSsl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class LastResponse
    {
        [JsonProperty("code")]
        public object Code { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }
    }

    public partial class PingEvent
    {
        public static PingEvent FromJson(string json) => JsonConvert.DeserializeObject<PingEvent>(json, Converter.Settings);

        public const string EventString = "ping";
    }
}
