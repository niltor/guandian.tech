using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MSDev.DB.Entities
{
    /// <summary>
    /// 商品类
    /// </summary>
    public class Commodity : Model
    {
        /// <summary>
        /// 编号
        /// </summary>
        [MaxLength(32)]
        public string SerialNumber { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 商品类型:实物、虚拟(服务)
        /// </summary>
        [MaxLength(32)]
        public string Type { get; set; }
        /// <summary>
        /// 服务型商品对应目标Id
        /// </summary>
        public Guid? TargetId { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginPrice { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 当前数量 
        /// </summary>
        public int CurrentNumber { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(4000)]
        public string Description { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        [MaxLength(200)]
        public string Thumbnail { get; set; }
        /// <summary>
        /// 目录id
        /// </summary>
        public Guid? CategoryId { get; set; }
    }
}
