using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MSDev.DB.Entities
{
    /// <summary>
    /// 本站视频
    /// </summary>
    public class Video
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 关联的博客
        /// </summary>
        [ForeignKey("BlogId")]
        public Blog Blog { get; set; }
        /// <summary>
        /// 关联的习题
        /// </summary>
        [ForeignKey("PracticeId")]
        public Practice Practice { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public Catalog Catalog { get; set; }
        [MaxLength(256)]
        public string Url { get; set; }
        [MaxLength(16)]
        public string Duration { get; set; }
        [MaxLength(32)]
        public string Author { get; set; }
        [MaxLength(128)]
        public string Tags { get; set; }
        [MaxLength(32)]
        public string Status { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsRecommend { get; set; }
        public int? Views { get; set; }
    }
}
