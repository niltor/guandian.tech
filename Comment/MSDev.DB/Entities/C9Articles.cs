using System;

namespace MSDev.DB.Entities

{
    public partial class C9Articles
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Duration { get; set; }
        public string SeriesTitle { get; set; }
        public string SeriesTitleUrl { get; set; }
        public string SourceUrl { get; set; }
        public int? Status { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Title { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
