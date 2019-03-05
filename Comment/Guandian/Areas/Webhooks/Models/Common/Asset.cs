using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Asset
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("browser_download_url")]
        public string BrowserDownloadUrl { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("size")]
        public long? Size { get; set; }

        [JsonProperty("download_count")]
        public long? DownloadCount { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]
         public DateTime?  CreatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("updated_at")]
         public DateTime?  UpdatedAt { get; set; }

        [JsonProperty("uploader")]
        public User Uploader { get; set; }
    }
}
