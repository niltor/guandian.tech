using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 贡献关联
    /// </summary>
    public class Contribution : BaseDb
    {
        public User User { get; set; }
        public Practknow Practknow { get; set; }
    }
}
