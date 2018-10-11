using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MSDev.DB.Entities
{
    /// <summary>
    /// 活动
    /// </summary>
    public class Activity
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 组织方，发起人
        /// </summary>
        public User Organizer { get; set; }
        [MaxLength(64)]
        public string Title { get; set; }
        [MaxLength(4000)]
        public string Content { get; set; }
        [MaxLength(32)]
        public string Status { get; set; }
        /// <summary>
        /// 活动形式类型
        /// </summary>
        [MaxLength(32)]
        public string ActivityType { get; set; }
        /// <summary>
        /// 在线链接
        /// </summary>
        [MaxLength(256)]
        public string OnlineUrl { get; set; }
        [MaxLength(256)]
        public string ThumbnailUrl { get; set; }
        public int CurrentPeopleNumber { get; set; }
        public int MaxPeopleNumber { get; set; }
        [MaxLength(256)]
        public string Address { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        public int? Views { get; set; }

        public ICollection<UserActivity> UserActivity { get; set; }
    }
}
