using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotionDetectorWebApi.Controllers.Communication
{
    public class PushSubscriptionRequest
    {
        public string Endpoint { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }
}
