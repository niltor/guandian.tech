using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 文件结点
    /// </summary>
    public class FileNode : BaseDb
    {
        /// <summary>
        /// 是否为文件/文件夹
        /// </summary>
        public bool IsFile { get; set; } = true;
        /// <summary>
        /// 文件名
        /// </summary>
        [Display(Name = "名称")]
        [MaxLength(200)]
        public string FileName { get; set; }
        /// <summary>
        /// SHA值 
        /// </summary>
        [MaxLength(200)]
        public string SHA { get; set; }
        /// <summary>
        /// github链接
        /// </summary>
        [MaxLength(500)]
        public string GithubLink { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [MaxLength(200)]
        [Display(Name = "路径")]
        public string Path { get; set; }
        /// <summary>
        /// 父结点
        /// </summary>
        public FileNode ParentNode { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public ICollection<FileNode> ChildrenNodes { get; set; }
        /// <summary>
        /// 践识
        /// </summary>
        public ICollection<Practknow> Practknows { get; set; }
    }
}
