using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class LabelEvent
    {
        /// <summary>
        ///  Can be "created", "edited", or "deleted".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("label")]
        public Label Label { get; set; }

        //TODO: [JsonProperty("changes")]

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }

    public partial class LabelEvent
    {
        public static LabelEvent FromJson(string json) => JsonConvert.DeserializeObject<LabelEvent>(json, Converter.Settings);

        public const string EventString = "label";
    }
}
