using Guandian.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
