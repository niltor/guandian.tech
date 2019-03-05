using GithubWebhook.Common;
using Newtonsoft.Json;

namespace GithubWebhook.Events
{
    /// <summary>
    /// watch
    /// </summary>
    public partial class WatchEvent
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("sender")]
        public User Sender { get; set; }
    }
    

    public partial class WatchEvent
    {
        public static WatchEvent FromJson(string json) => JsonConvert.DeserializeObject<WatchEvent>(json, Converter.Settings);

        public const string EventString = "watch";
    }
   
}
