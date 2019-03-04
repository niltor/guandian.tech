using System;
using System.ComponentModel.DataAnnotations;

namespace Guandian.Data.Entity
{
    public class BaseDb
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "创建时间")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        [Display(Name = "更新时间")]
        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        public Status Status { get; set; } = Status.Default;
    }

    public enum Status
    {
        /// <summary>
        /// 默认的[新的，无效的]
        /// </summary>
        [Display(Name = "New")]
        Default,
        /// <summary>
        /// 有效的
        /// </summary>
        [Display(Name = "有效")]
        Valid,
        /// <summary>
        /// 已推送到微信公众号
        /// </summary>
        [Display(Name = "推")]
        IsPublishedMP,
        /// <summary>
        /// 废弃的
        /// </summary>
        [Display(Name = "废弃")]
        Obsolete
    }
}
