using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Membership
    {
        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("organization_url")]
        public string OrganizationUrl { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
   
}
