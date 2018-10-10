using System;
using System.Collections.Generic;
using System.Text;

namespace MSDev.DB.Community
{
    /// <summary>
    /// 喜欢程度
    /// </summary>
    public class LikeAnalysis:BaseModel
    {
        /// <summary>
        /// 对象id
        /// </summary>
        public Guid ObjectId { get; set; }
        /// <summary>
        /// 对象类型
        /// </summary>
        public string ObjectType { get; set; }
        /// <summary>
        /// 喜欢数量
        /// </summary>
        public long LikeNumber { get; set; }

        /// <summary>
        /// 讨厌数量 
        /// </summary>
        public long UnLinkeNumber { get; set; }

        /// <summary>
        /// 赞同数量 
        /// </summary>
        public long AgreeNumber { get; set; }

        /// <summary>
        /// 浏览数量
        /// </summary>
        public long ViewNumber { get; set; }
    }
}
