using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Commit
    {
        [JsonProperty("tree")]
        public Tree Tree { get; set; }

        [JsonProperty("comment_count")]
        public long? CommentCount { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tree_id")]
        public string TreeId { get; set; }

        [JsonProperty("distinct")]
        public bool? Distinct { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("timestamp")]

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        public DateTime? Timestamp { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("author")]
        public User Author { get; set; }

        [JsonProperty("committer")]
        public User Committer { get; set; }

        [JsonProperty("added")]
        public object[] Added { get; set; }

        [JsonProperty("removed")]
        public object[] Removed { get; set; }

        [JsonProperty("modified")]
        public string[] Modified { get; set; }

        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("comments_url")]
        public string CommentsUrl { get; set; }

        [JsonProperty("commit")]
        public Commit CommitCommit { get; set; }

        [JsonProperty("parents")]
        public Tree[] Parents { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("files")]
        public File[] Files { get; set; }
    }

}
