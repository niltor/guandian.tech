using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MSDev.DB.Entities
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order : Model
    {
        /// <summary>
        /// 编号
        /// </summary>
        [MaxLength(32)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 关联商品
        /// </summary>
        public Commodity Commodity { get; set; }

        public Guid CommodityId { get; set; }

        /// <summary>
        /// 关联用户
        /// </summary>
        public User User { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 付款价格
        /// </summary>
        public decimal PayPrice { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [NotMapped]
        public Enum OrderStatus { get; set; }


    }
}
