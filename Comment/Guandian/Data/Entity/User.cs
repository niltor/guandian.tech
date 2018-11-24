using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Guandian.Data.Entity
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [PersonalData]
        [MaxLength(100)]
        public string RealName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [PersonalData]
        [MaxLength(100)]
        public string NickName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [PersonalData]
        [StringLength(18, ErrorMessage = "需要18位的有效身份证格式", MinimumLength = 18)]
        public string IdentityCard { get; set; }
        [DataType(DataType.Date)]
        [PersonalData]
        public DateTime Birthday { get; set; }
        public ICollection<Respository> Respositories{ get; set; }

    }
}