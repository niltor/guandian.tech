using System;

namespace Guandian.Areas.Admin.Models
{
    public class BingNewsEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Provider { get; set; }

        public DateTime DatePublished { get; set; }

        public string Category { get; set; }
    }
}
