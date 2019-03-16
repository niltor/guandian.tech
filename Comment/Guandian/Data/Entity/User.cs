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
        /// githubId
        /// </summary>
        [MaxLength(20)]
        public string GitId { get; set; }
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
        public ICollection<Repository> Respositories { get; set; }
        public ICollection<Practknow> Practknows { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ReviewComment> ReviewComments { get; set; }
        /// <summary>
        /// 是否fork践识
        /// </summary>
        public bool IsForkPractknow { get; set; } = false;
        /// <summary>
        /// 是否为成员
        /// </summary>
        public bool IsMember { get; set; } = false;
    }
}