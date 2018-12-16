using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guandian.Models.Forms
{
    /// <summary>
    /// 践识添加表单
    /// </summary>
    public class AddPractknowForm
    {
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Summary { get; set; }
        public Guid? NodeId { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 目录
        /// </summary>
        public string Path { get; set; } = "未分类/";
    }
}
