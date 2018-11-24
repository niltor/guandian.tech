using System.Collections.Generic;
using Guandian.Data.Entity;

namespace Guandian.Models.PractknowView
{
    public class PractknowIndexView
    {
        /// <summary>
        /// 节点导航
        /// </summary>
        public List<FileNode> NodeTree { get; set; }
        /// <summary>
        /// 当前节点
        /// </summary>
        public List<FileNode> CurrentNodes { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public Practknow Practknow { get; set; }
    }
}
