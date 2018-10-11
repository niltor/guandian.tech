using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MSDev.DB.Entities
{
    public class EventVideo
    {
        public Guid Id { get; set; }
        public C9Event C9Event { get; set; }
        public Guid C9EventId { get; set; }
        [MaxLength(256)]
        public string Author { get; set; }
        public DateTime CreatedTime { get; set; }
        [MaxLength(5000)]
        public string Description { get; set; }
        public string Duration { get; set; }
        [MaxLength(16)]
        public string Language { get; set; }
        [MaxLength(256)]
        public string Mp3Url { get; set; }
        public string Mp4HigUrl { get; set; }
        [MaxLength(256)]
        public string Mp4LowUrl { get; set; }
        [MaxLength(256)]
        public string Mp4MidUrl { get; set; }
        [MaxLength(64)]
        public string SeriesType { get; set; }
        [MaxLength(1000)]
        public string VideoEmbed { get; set; }
        [MaxLength(256)]
        public string SeriesTitle { get; set; }
        [MaxLength(256)]
        public string SeriesTitleUrl { get; set; }
        [MaxLength(256)]
        public string SourceUrl { get; set; }
        public int? Status { get; set; }
        [MaxLength(500)]
        public string Tags { get; set; }
        /// <summary>
        /// 字幕链接内容
        /// </summary>
        public string Caption { get; set; }
        [MaxLength(256)]
        public string ThumbnailUrl { get; set; }
        [MaxLength(256)]
        public string Title { get; set; }
        public DateTime UpdatedTime { get; set; }
        [MaxLength(128)]
        public string DateString { get; set; }
        public int? Views { get; set; }
    }
}
