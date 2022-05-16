using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTO
{
    public class PostDTO
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

    public class PostRequestDto
    {
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
