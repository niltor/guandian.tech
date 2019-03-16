using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guandian.Models.PractknowView
{
    /// <summary>
    /// 标题搜索
    /// </summary>
    public class SearchTitle
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
