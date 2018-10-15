using System;

namespace MSBlogsFunction.Models
{
    public class BlogForm
    {
        /// <summary>
        /// 英文标题
        /// </summary>
        public string TitleEn { get; set; }
        /// <summary>
        /// 英文内容
        /// </summary>
        public string ContentEn { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// 目录标签
        /// </summary>
        public string Categories { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 概要
        /// </summary>
        public string Summary { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
