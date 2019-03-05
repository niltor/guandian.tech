using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Invitation
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("email")]
        public object Email { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("invitee")]
        public User Invitee { get; set; }

        [JsonProperty("inviter")]
        public User Inviter { get; set; }

        [JsonProperty("permissions")]
        public string Permissions { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]        
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }
}
