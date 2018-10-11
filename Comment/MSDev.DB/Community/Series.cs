using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MSDev.DB.Community
{
    /// <summary>
    /// 系列
    /// </summary>
    public class Series:BaseModel
    {

        public ICollection<Blog> Blogs { get; set; }

        /// <summary>
        /// 目录名称
        /// </summary>
        [MaxLength(32)]
        public string CatalogName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Value { get; set; }


    }
}
