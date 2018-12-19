using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 践识
    /// </summary>
    public class Practknow : BaseDb
    {
        [MaxLength(200)]
        [Display(Name = "标题")]
        public string Title { get; set; }
        [MaxLength(120)]
        [Display(Name = "作者")]
        public string AuthorName { get; set; }
        public User User { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
        /// <summary>
        /// 浏览数量 
        /// </summary>
        public int ViewNunmber { get; set; } = 0;
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        [MaxLength(200)]
        [Display(Name = "关键词")]
        public string Keywords { get; set; }
        /// <summary>
        /// 概要
        /// </summary>
        [MaxLength(1000)]
        [Display(Name = "概要")]
        public string Summary { get; set; }
        /// <summary>
        /// github SHA
        /// </summary>
        [MaxLength(200)]
        public string SHA { get; set; }
        /// <summary>
        /// pull request number
        /// </summary>
        public int PRNumber { get; set; }
        /// <summary>
        /// 文件结点
        /// </summary>
        public FileNode FileNode { get; set; }
    }
}
