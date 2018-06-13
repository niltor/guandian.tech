using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Comment.Data.Entity
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [StringLength(18, ErrorMessage = "需要18位的有效身份证格式", MinimumLength = 18)]
        public string IdentityCard { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
    }
}