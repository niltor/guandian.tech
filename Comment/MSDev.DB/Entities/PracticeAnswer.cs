using System;
using System.ComponentModel.DataAnnotations;

namespace MSDev.DB.Entities
{
    public class PracticeAnswer
    {
        public Guid Id { get; set; }

        public User User { get; set; }
        public Practice Practice { get; set; }

        [MaxLength(4000, ErrorMessage = "超过字数限制")]
        public string Content { get; set; }
        public int? AgreeNumber { get; set; }
        public int? DisagreeNumber { get; set; }

        [MaxLength(32)]
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}