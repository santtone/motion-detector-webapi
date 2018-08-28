using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotionDetectorWebApi.Models
{
    public class MotionFile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string ThumbnailLink { get; set; }
        public DateTime? Date { get; set; }
        public string MimeType { get; set; }
    }
}