using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Team
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("privacy")]
        public string Privacy { get; set; }

        [JsonProperty("permission")]
        public string Permission { get; set; }

        [JsonProperty("members_url")]
        public string MembersUrl { get; set; }

        [JsonProperty("repositories_url")]
        public string RepositoriesUrl { get; set; }

        [JsonProperty("parent")]
        public object Parent { get; set; }

        [JsonProperty("members_count")]
        public long? MembersCount { get; set; }

        [JsonProperty("repos_count")]
        public long? ReposCount { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }
    }
}
