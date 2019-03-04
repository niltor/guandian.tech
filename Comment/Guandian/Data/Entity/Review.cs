using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public string Url { get; set; }
        [MaxLength(300)]
        public string DiffUrl { get; set; }
        [MaxLength(300)]
        public string CommitsUrl { get; set; }
        public int Number { get; set; }
        [MaxLength(100)]
        public string MergeCommitSha { get; set; }
        public DateTime MergeTime { get; set; }
        public ReviewStatus MergeStatus { get; set; }
        public ICollection<ReviewComment> ReviewComments { get; set; }
        public User User { get; set; }

    }

    public enum ReviewStatus
    {
        Open,
        Close
    }
}
