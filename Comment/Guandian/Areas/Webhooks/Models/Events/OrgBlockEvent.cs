using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class OrgBlockEvent
    {
        /// <summary>
        /// Can be "blocked" or "unblocked".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("blocked_user")]
        public User BlockedUser { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }
   
    public partial class OrgBlockEvent
    {
        public static OrgBlockEvent FromJson(string json) => JsonConvert.DeserializeObject<OrgBlockEvent>(json, Converter.Settings);
        public const string EventString = "org_block";
    }
    
}
