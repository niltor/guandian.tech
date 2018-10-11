using System;
using System.ComponentModel.DataAnnotations;

namespace MSDev.DB.Entities
{
    public partial class RssNews
    {
        public int Id { get; set; }
        [MaxLength(256)]
        public string Author { get; set; }
        [MaxLength(256)]
        public string Categories { get; set; }
        public DateTime CreateTime { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdateTime { get; set; }
        [MaxLength(512)]
        public string Link { get; set; }
        public string MobileContent { get; set; }
        public int PublishId { get; set; }
        public int Status { get; set; }

        [MaxLength(512)]
        public string Title { get; set; }
        public int Type { get; set; }
        /// <summary>
        /// 中文标题
        /// </summary>
        [MaxLength(250)]
        public string TitleCn { get; set; }

        /// <summary>
        /// 中文内容
        /// </summary>
        public string Content { get; set; }
    }
}
