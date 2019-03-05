using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Comment
    {
        [JsonProperty("pull_request_review_id")]
        public long? PullRequestReviewId { get; set; }

        [JsonProperty("in_reply_to_id")]
        public long? InReplyToId { get; set; }

        [JsonProperty("diff_hunk")]
        public string DiffHunk { get; set; }

        [JsonProperty("original_position")]
        public long? OriginalPosition { get; set; }

        [JsonProperty("original_commit_id")]
        public string OriginalCommitId { get; set; }

        [JsonProperty("pull_request_url")]
        public string PullRequestUrl { get; set; }

        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("position")]
        public long? Position { get; set; }

        [JsonProperty("line")]
        public long? Line { get; set; }

        [JsonProperty("commit_id")]
        public string CommitId { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]        
        public DateTime? CreatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("updated_at")]        
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("issue_url")]
        public string IssueUrl { get; set; }
    }

}
