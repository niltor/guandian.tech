namespace MSDev.DB.Entities
{
    public class BingNews : Model
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容概要
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 来源地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 缩略图链接
        /// </summary>
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Provider { get; set; }

    }
}
