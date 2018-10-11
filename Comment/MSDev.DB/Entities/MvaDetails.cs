using System;
using System.ComponentModel.DataAnnotations;

namespace MSDev.DB.Entities
{
    public class MvaDetails : Model
    {

        [MaxLength(32)]
        public string MvaId { get; set; }
        [MaxLength(3)]
        public int Sequence { get; set; }

        public MvaVideos MvaVideo { get; set; }

        [MaxLength(128)]
        public string  Title { get; set; }

        [MaxLength(500)]
        public string SourceUrl { get; set; }

        [MaxLength(500)]
        public string LowDownloadUrl { get; set; }
        [MaxLength(500)]

        public string MidDownloadUrl { get; set; }
        [MaxLength(500)]

        public string HighDownloadUrl { get; set; }

        [DataType(DataType.Duration)]
        public DateTime Duration { get; set; }

    }
}
