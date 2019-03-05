using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 审核
    /// </summary>
    public class Review : BaseDb
    {
        [MaxLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// 提交内容
        /// </summary>
        [MaxLength(2000)]
        public string Body { get; set; }
        [MaxLength(300)]
        public string Url { get; set; }
        [MaxLength(300)]
        public string HtmlUrl { get; set; }
        [MaxLength(300)]
        public string DiffUrl { get; set; }
        [MaxLength(300)]
        public string CommitsUrl { get; set; }
        public int Number { get; set; }
        [MaxLength(100)]
        public string MergeCommitSha { get; set; }
        public bool Merged { get; set; }
        public DateTimeOffset MergeTime { get; set; }
        public ReviewStatus MergeStatus { get; set; }
        public ICollection<ReviewComment> ReviewComments { get; set; }
        public User User { get; set; }
    }

    public enum ReviewStatus
    {
        Open,
        Closed
    }
}
