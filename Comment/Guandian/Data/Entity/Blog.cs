using System.ComponentModel.DataAnnotations;

namespace Guandian.Data.Entity
{
    public class Blog : Article
    {
        /// <summary>
        /// 英文标题
        /// </summary>
        [MaxLength(200)]
        public string TitleEn { get; set; }
        /// <summary>
        /// 英文内容
        /// </summary>
        public string ContentEn { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        [MaxLength(300)]
        public string Link { get; set; }
        /// <summary>
        /// 目录标签
        /// </summary>
        [MaxLength(300)]
        public string Categories { get; set; }
        /// <summary>
        /// 是否推送到微信
        /// </summary>
        public bool IsPublishMP { get; set; } = false;
        /// <summary>
        /// 缩略图
        /// </summary>
        [MaxLength(200)]
        public string Thumbnail { get; set; }
    }
}
