using System.ComponentModel.DataAnnotations;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 审核信息
    /// </summary>
    public class ReviewComment : BaseDb
    {
        [MaxLength(300)]
        public string HtmlUrl { get; set; }
        [MaxLength(2000)]
        public string Content { get; set; }
        public User User { get; set; }
        public Review Review { get; set; }
    }
}
