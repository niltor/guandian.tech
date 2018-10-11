using System;

namespace MSDev.DB.Entities

{
    public partial class Sources
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Hash { get; set; }
        public string Name { get; set; }
        public Guid? ResourceId { get; set; }
        public int? Status { get; set; }
        public string Tag { get; set; }
        public string Type { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Url { get; set; }

        public Resource Resource { get; set; }
    }
}
