using System;
using Newtonsoft.Json;

namespace MotionDetectorWebApi.Controllers.Communication
{
    public class PushMessage
    {
        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}