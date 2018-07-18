using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MotionDetectorWebApi.Models
{
    [Table("UserPushSubscription")]
    public class UserPushSubscription
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string EndPoint { get; set; }
        public string P256Dh { get; set; }
        public string Auth { get; set; }

        public UserPushSubscription() { }

        public UserPushSubscription(string endPoint, string p256Dh, string auth)
        {
            EndPoint = endPoint;
            P256Dh = p256Dh;
            Auth = auth;
        }
    }

}
