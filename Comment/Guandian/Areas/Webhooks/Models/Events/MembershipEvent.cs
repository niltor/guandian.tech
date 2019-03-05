using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class MembershipEvent
    {
        /// <summary>
        ///  Can be "added" or "removed".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>
        /// Currently, can only be "team".
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("member")]
        public User Member { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }

        [JsonProperty("team")]
        public Team Team { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }
    }

    public partial class MembershipEvent
    {
        public static MembershipEvent FromJson(string json) => JsonConvert.DeserializeObject<MembershipEvent>(json, Converter.Settings);

        public const string EventString = "membership";
    }
}
