using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSDev.DB.Entities
{
    /// <summary>
    /// 练习题目
    /// </summary>
    public class Practice
    {
        public Guid Id { get; set; }
        public Catalog Catalog { get; set; }


        public Video Video { get; set; }
        public Blog Blog { get; set; }
        public ICollection<UserPractice> UserPractice { get; set; }

        [MaxLength(64)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Content { get; set; }
        [MaxLength(32)]
        public string Status { get; set; }

        public ICollection<PracticeAnswer> PracticeAnswer { get; set; }
        public int? Views { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }


    }
}