using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSDev.DB.Entities
{
    /// <summary>
    /// 资源
    /// </summary>
    public partial class Resource
    {
        public Resource()
        {
            Sources = new HashSet<Sources>();
            IsRecommend = false;
        }
        public Guid Id { get; set; }
        public string AbsolutePath { get; set; }
        public Guid? CatalogId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int? Status { get; set; }
        public int Type { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Imgurl { get; set; }
        /// <summary>
        /// 提供方
        /// </summary>
        [MaxLength(128)]
        public string Provider { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [MaxLength(128)]
        public string Tag { get; set; }

        public int Language { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int ViewNumber { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        
        public bool IsRecommend { get; set; }

        public Catalog Catalog { get; set; }
        public ICollection<Sources> Sources { get; set; }
    }
}
