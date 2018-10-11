using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MSDev.DB.Entities
{
    public class C9Event
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 当前活动主题名
        /// </summary>
        [MaxLength(128)]
        public string TopicName { get; set; }
        /// <summary>
        /// 活动类型名称
        /// </summary>
        [MaxLength(64)]
        public string EventName { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [MaxLength(16)]
        public string Language { get; set; }
        [MaxLength(32)]
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 活动时间
        /// </summary>
        [MaxLength(64)]
        public string EventDate { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        [MaxLength(256)]
        public string ThumbnailUrl { get; set; }
        [MaxLength(256)]
        public string SourceUrl { get; set; }


        public ICollection<EventVideo> EventVideos { get; set; }
    }
}
