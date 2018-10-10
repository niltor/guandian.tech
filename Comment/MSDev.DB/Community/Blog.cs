using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MSDev.DB.Community
{
    /// <summary>
    /// 博客,markdown
    /// </summary>
    public class Blog:BaseModel
    {
        /// <summary>
        /// 系列
        /// </summary>
        public Series Series { get; set; }
        public Guid SeriesId { get; set; }

        [MaxLength(128)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        /// <summary>
        /// 类型：私有，公开
        /// </summary>
        public string Type { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 作者名称
        /// </summary>
        [MaxLength(64)]
        public string AuthorName { get; set; }
        /// <summary>
        /// 目录：新闻、技术、经历等
        /// </summary>
        [MaxLength(32)]
        public string CatalogName { get; set; }
        /// <summary>
        /// 作者唯一标识 
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [MaxLength(256)]
        public string Tags { get; set; }

        /// <summary>
        /// 在线地址
        /// </summary>
        [MaxLength(256)]
        public string SourceUrl { get; set; }

        public bool IsRecommend { get; set; }

    }
}
