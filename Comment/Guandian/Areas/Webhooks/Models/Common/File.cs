using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class File
    {
        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("additions")]
        public long? Additions { get; set; }

        [JsonProperty("deletions")]
        public long? Deletions { get; set; }

        [JsonProperty("changes")]
        public long? Changes { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("raw_url")]
        public string RawUrl { get; set; }

        [JsonProperty("blob_url")]
        public string BlobUrl { get; set; }

        [JsonProperty("patch")]
        public string Patch { get; set; }
    }
}
