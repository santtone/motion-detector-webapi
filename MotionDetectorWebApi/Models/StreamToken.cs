using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotionDetectorWebApi.Models
{
    public class StreamToken
    {
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}