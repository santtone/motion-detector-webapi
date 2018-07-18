using MotionDetectorWebApi.Models;
using System.Threading.Tasks;
using MotionDetectorWebApi.Controllers.Communication;
using WebPush;

namespace MotionDetectorWebApi.Services
{
    public interface IWebPushService
    {
        VapidDetails GetVapidDetails(int userId);
        Task<UserPushSubscription> Create(int userId, string endpoint, string p256dh, string auth);
        Task<PushMessage> SendNotificationForUser(int userId, string payload);
    }
}