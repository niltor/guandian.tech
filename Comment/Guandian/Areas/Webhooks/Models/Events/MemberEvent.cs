using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class MemberEvent
    {
        /// <summary>
        /// Can be one of "added", "deleted", or "edited"
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        //TODO: [JsonProperty("changes")]

        [JsonProperty("member")]
        public User Member { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class MemberEvent
    {
        public static MemberEvent FromJson(string json) => JsonConvert.DeserializeObject<MemberEvent>(json, Converter.Settings);

        public const string EventString = "member";
    }
}
