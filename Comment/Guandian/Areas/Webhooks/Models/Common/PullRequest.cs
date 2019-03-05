
using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class PullRequest
    {
        [JsonProperty("assignees")]
        public User[] Assignees { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("issue_url")]
        public string IssueUrl { get; set; }

        [JsonProperty("commits_url")]
        public string CommitsUrl { get; set; }

        [JsonProperty("review_comments_url")]
        public string ReviewCommentsUrl { get; set; }

        [JsonProperty("review_comment_url")]
        public string ReviewCommentUrl { get; set; }

        [JsonProperty("comments_url")]
        public string CommentsUrl { get; set; }

        [JsonProperty("statuses_url")]
        public string StatusesUrl { get; set; }

        [JsonProperty("number")]
        public long? Number { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("assignee")]
        public User Assignee { get; set; }

        [JsonProperty("labels")]
        public Label[] Labels { get; set; }

        [JsonProperty("milestone")]
        public Milestone Milestone { get; set; }

        [JsonProperty("locked")]
        public bool? Locked { get; set; }

        [JsonProperty("active_lock_reason")]
        public string ActiveLockReason { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("created_at")]
         public DateTime?  CreatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("updated_at")]
         public DateTime?  UpdatedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("closed_at")]
         public DateTime?  ClosedAt { get; set; }

        [JsonConverter(typeof(GithubInconsistentDateTimeTypeConverter))]
        [JsonProperty("merged_at")]
         public DateTime?  MergedAt { get; set; }

        [JsonProperty("head")]
        public Base Head { get; set; }

        [JsonProperty("base")]
        public Base Base { get; set; }

        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("merge_commit_sha")]
        public string MergeCommitSha { get; set; }

        [JsonProperty("merged")]
        public bool? Merged { get; set; }

        [JsonProperty("mergeable")]
        public bool? Mergeable { get; set; }

        [JsonProperty("merged_by")]
        public User MergedBy { get; set; }

        [JsonProperty("comments")]
        public long? Comments { get; set; }

        [JsonProperty("commits")]
        public long? Commits { get; set; }

        [JsonProperty("additions")]
        public long? Additions { get; set; }

        [JsonProperty("deletions")]
        public long? Deletions { get; set; }

        [JsonProperty("changed_files")]
        public long? ChangedFiles { get; set; }

        [JsonProperty("maintainer_can_modify")]
        public bool? MaintainerCanModify { get; set; }

        [JsonProperty("mergeable_state")]
        public string MergeableState { get; set; }

        [JsonProperty("review_comments")]
        public long? ReviewComments { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("diff_url")]
        public string DiffUrl { get; set; }

        [JsonProperty("patch_url")]
        public string PatchUrl { get; set; }
    }

}
