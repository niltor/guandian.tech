using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSDev.DB.Entities

{
    public partial class MvaVideos
    {
        public List<MvaDetails> Details { get; set; }
        [MaxLength(4000)]
        public string  DetailDescription { get; set; }
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string AuthorCompany { get; set; }
        public string AuthorJobTitle { get; set; }
        public string CourseDuration { get; set; }
        public string CourseImage { get; set; }
        public string CourseLevel { get; set; }
        public string CourseNumber { get; set; }
        public string CourseStatus { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }
        public int? MvaId { get; set; }
        public int? ProductPackageVersionId { get; set; }
        public string SourceUrl { get; set; }
        public int? Status { get; set; }
        public string Tags { get; set; }
        public string Technologies { get; set; }
        public string Title { get; set; }
        public bool IsRecommend { get; set; }
        public DateTime UpdatedTime { get; set; }

        public int? Views { get; set; }
    }
}
