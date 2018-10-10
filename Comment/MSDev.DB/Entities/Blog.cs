using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MSDev.DB.Entities
{
    /// <summary>
    /// 博客,markdown
    /// </summary>
    public class Blog
    {
        public Guid Id { get; set; }

        [MaxLength(128)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        [ForeignKey("VideoId")]
        public Video Video { get; set; }

        [ForeignKey("PracticeId")]
        public Practice Practice { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// 作者名称
        /// </summary>
        [MaxLength(64)]
        public string AuthorName { get; set; }
        /// <summary>
        /// 作者唯一标识 
        /// </summary>
        [MaxLength(128)]
        public string AuthorId { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [MaxLength(256)]
        public string Tags { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public Catalog Catalog { get; set; }

        /// <summary>
        /// 在线地址
        /// </summary>
        [MaxLength(256)]
        public string SourceUrl { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsRecommend { get; set; }

        public int? Views { get; set; }
        [MaxLength(64)]
        public string Status { get; set; }

    }
}
