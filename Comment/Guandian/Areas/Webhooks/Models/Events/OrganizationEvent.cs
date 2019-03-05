using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class OrganizationEvent
    {
        /// <summary>
        /// Can be one of: "member_added", "member_removed", or "member_invited".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("invitation")]
        public Invitation Invitation { get; set; }

        [JsonProperty("membership")]
        public Membership Membership { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }


    public partial class OrganizationEvent
    {
        public static OrganizationEvent FromJson(string json) => JsonConvert.DeserializeObject<OrganizationEvent>(json, Converter.Settings);

        public const string EventString = "organization";
    }
   
}
