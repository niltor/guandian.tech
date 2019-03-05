using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class DeleteEvent
    {
        [JsonProperty("ref")]
        public string Ref { get; set; }

        /// <summary>
        /// Can be "branch" or "tag"
        /// </summary>
        [JsonProperty("ref_type")]
        public string RefType { get; set; }

        [JsonProperty("pusher_type")]
        public string PusherType { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }

    }

    public partial class DeleteEvent
    {
        public static DeleteEvent FromJson(string json) => JsonConvert.DeserializeObject<DeleteEvent>(json, Converter.Settings);

        public const string EventString = "delete";
    }
}
