using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogAPI.Repository.Models
{
    public partial class TblPost
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public string MetaTitle { get; set; }
        public string Category { get; set; }
        public string Imagepath { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public int Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? Views { get; set; }
        public string Log { get; set; }
    }
}
