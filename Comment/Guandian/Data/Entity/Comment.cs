using System.ComponentModel.DataAnnotations;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 评论
    /// </summary>
    public class Comment : BaseDb
    {
        public Author Author { get; set; }
        public Practknow Practknow { get; set; }
        [MaxLength(1000)]
        public string Content { get; set; }

        [MaxLength(500)]
        public string Link { get; set; }
    }
}
