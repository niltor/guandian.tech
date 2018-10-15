using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Guandian.Data.Entity
{
    public class BaseDb
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        public Status Status { get; set; } = Status.Default;
    }

    public enum Status
    {
        /// <summary>
        /// 默认的[新的，无效的]
        /// </summary>
        Default,
        /// <summary>
        /// 有效的
        /// </summary>
        Valid,
        /// <summary>
        /// 已推送到微信公众号
        /// </summary>
        IsPublishedMP


    }
}
