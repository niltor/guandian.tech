using System.ComponentModel.DataAnnotations;

namespace Comment.Data.Entity
{
    public class Blog : Article
    {
        /// <summary>
        /// 英文标题
        /// </summary>
        [MaxLength(120)]
        public string TitleEn { get; set; }
        /// <summary>
        /// 英文内容
        /// </summary>
        public string ContentEn { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        [MaxLength(250)]
        public string Link { get; set; }
        /// <summary>
        /// 目录标签
        /// </summary>
        [MaxLength(100)]
        public string Categories { get; set; }
    }
}
