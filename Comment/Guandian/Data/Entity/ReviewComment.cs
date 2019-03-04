using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 审核信息
    /// </summary>
    public class ReviewComment : BaseDb
    {
        [MaxLength(300)]
        public string Url { get; set; }
        [MaxLength(100)]
        public string Sha { get; set; }
        [MaxLength(2000)]
        public string Content { get; set; }
        public User User { get; set; }
    }
}
