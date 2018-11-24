using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : BaseDb
    {
        [MaxLength(200)]
        [Display(Name = "标题")]
        public string Title { get; set; }
        [MaxLength(120)]
        [Display(Name = "作者")]
        public string AuthorName { get; set; }
        public Author Author { get; set; }
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
    }
}
