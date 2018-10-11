using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MSDev.DB.Entities
{
    /// <summary>
    /// 用户拥有的服务,由购买等行为产生
    /// </summary>
    public class UserServices : Model
    {
        /// <summary>
        /// 服务类型
        /// </summary>
        [MaxLength(30)]
        public string Type { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 服务对象Id
        /// </summary>
        public Guid TargetId { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// 目标对象
        /// </summary>
        [NotMapped]
        public Object Target { get; set; }

    }
}
