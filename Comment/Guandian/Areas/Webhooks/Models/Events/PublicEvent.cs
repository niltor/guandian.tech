using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    public partial class PublicEvent
    {
        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }
   

    public partial class PublicEvent
    {
        public static PublicEvent FromJson(string json) => JsonConvert.DeserializeObject<PublicEvent>(json, Converter.Settings);

        public const string EventString = "public";
    }

}
