using System.ComponentModel.DataAnnotations;

namespace Comment.Data.Entity
{
    /// <summary>
    /// 第三方新闻资讯
    /// </summary>
    public class News : BaseDb
    {
        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// 作者名称
        /// </summary>
        [MaxLength(100)]
        public string AuthorName { get; set; }
        /// <summary>
        /// 新闻内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 内容概要
        /// </summary>
        [MaxLength(400)]
        public string Description { get; set; }
        /// <summary>
        /// 来源地址
        /// </summary>
        [MaxLength(200)]
        public string Url { get; set; }
        /// <summary>
        /// 缩略图链接
        /// </summary>
        [MaxLength(200)]
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [MaxLength(100)]
        public string Tags { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        [MaxLength(100)]
        public string Provider { get; set; }
        /// <summary>
        /// 是否发布到公众号
        /// </summary>
        public bool IsPublishToMP { get; set; } = false;

    }
}
