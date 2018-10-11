using System;
using System.ComponentModel.DataAnnotations;

namespace MSDev.DB.Entities
{
    /// <summary>
    /// 存储常用配置
    /// </summary>
    public class Config
	{
		public Guid Id { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		[MaxLength(32)]
		public string Name { get; set; }

		/// <summary>
		/// 类型
		/// </summary>
		[MaxLength(32)]
		public string Type { get; set; }

		/// <summary>
		/// 值
		/// </summary>
		[MaxLength(4000)]
		public string Value { get; set; }
	}
}
