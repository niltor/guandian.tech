using Newtonsoft.Json;

namespace MSBlogsFunction.Models
{
    class BingTranslateResponse
    {
        [JsonProperty("resultSMT")]
        public string ResultSMT { get; set; }

        [JsonProperty("targetLanguage")]
        public string TargetLanguage { get; set; }

        [JsonProperty("resultNMT")]
        public string ResultNMT { get; set; }

        [JsonProperty("sourceLanguage")]
        public string SourceLanguage { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

    }
}
