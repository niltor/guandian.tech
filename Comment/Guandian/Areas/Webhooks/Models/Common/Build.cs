using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Build
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }

        [JsonProperty("pusher")]
        public User Pusher { get; set; }

        [JsonProperty("commit")]
        public string Commit { get; set; }

        [JsonProperty("duration")]
        public long? Duration { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]
         public DateTime?  CreatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("updated_at")]
         public DateTime?  UpdatedAt { get; set; }
    }


}
