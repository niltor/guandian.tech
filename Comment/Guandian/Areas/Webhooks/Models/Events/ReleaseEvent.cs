// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using GithubWebhook;
//
//    var data = ReleaseEvent.FromJson(jsonString);

using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class ReleaseEvent
    {
        /// <summary>
        /// Currently, can only be "published".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("release")]
        public Release Release { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }
   

    public partial class ReleaseEvent
    {
        public static ReleaseEvent FromJson(string json) => JsonConvert.DeserializeObject<ReleaseEvent>(json, Converter.Settings);

        public const string EventString = "release";
    }
}
