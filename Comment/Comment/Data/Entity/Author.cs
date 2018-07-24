using System.Collections.Generic;

namespace Comment.Data.Entity
{
    /// <summary>
    /// 作者
    /// </summary>
    public class Author : User
    {
        /// <summary>
        /// 文章列表
        /// </summary>
        public ICollection<Article> Articles { get; set; }

        /// <summary>
        /// 评论列表
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
    }
}