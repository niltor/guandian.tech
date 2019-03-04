using Guandian.Data.Entity;
using System.Collections.Generic;

namespace Guandian.Areas.Admin.Models
{

    public class FileNodesView
    {
        /// <summary>
        /// 路径
        /// </summary>
        public List<FileNode> Path { get; set; }
        public List<FileNode> FileNodes { get; set; }
    }
}
