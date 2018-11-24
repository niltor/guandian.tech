using System.ComponentModel.DataAnnotations;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 用户仓库
    /// </summary>
    public class Respository : BaseDb
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 标识 :仓库标识 
        /// </summary>
        [MaxLength(50)]
        public string Tag { get; set; }
        public User User { get; set; }

    }
}
