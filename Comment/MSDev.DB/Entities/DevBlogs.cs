using System;

namespace MSDev.DB.Entities

{
    public partial class DevBlogs
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Link { get; set; }
        public string SourcContent { get; set; }
        public string SourceTitle { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
