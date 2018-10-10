using System;
using System.ComponentModel.DataAnnotations;
using WebAdmin.DB.Utils;

namespace MSDev.DB.Entities

{
    public class Model
    {
        /// <summary>
        /// Guid
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 状态值
        /// </summary>
        [MaxLength(32)]
        public string Status { get; set; } = StatusType.New;


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedTime { get; set; } = DateTime.Now;


        //public bool IsDelete { get; set; }
    }
}
