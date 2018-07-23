using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Comment.Data.Entity
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : Db
    {
        [MaxLength(60)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string AuthorName { get; set; }
        public Author Author { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
        /// <summary>
        /// 浏览数量 
        /// </summary>
        public int ViewNunmber { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [MaxLength(4000)]
        public string Content { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        [MaxLength(200)]
        public string Keywords { get; set; }
        /// <summary>
        /// 概要
        /// </summary>
        [MaxLength(500)]
        public string Summary { get; set; }
    }
}
