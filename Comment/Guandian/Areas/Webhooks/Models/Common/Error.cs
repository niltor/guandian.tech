using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Error
    {
        [JsonProperty("message")]
        public object Message { get; set; }
    }

   
}
