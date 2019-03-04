using System.ComponentModel.DataAnnotations;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 设置配置项
    /// </summary>
    public class Setting : BaseDb
    {
        [MaxLength(100)]
        public string Key { get; set; }
        [MaxLength(100)]
        public string Value { get; set; }
        [MaxLength(100)]
        public string Type { get; set; }
        public bool IsEffect { get; set; } = true;
    }
}
