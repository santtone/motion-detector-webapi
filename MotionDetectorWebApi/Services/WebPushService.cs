using MotionDetectorWebApi.Models;
using MotionDetectorWebApi.Repositories;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MotionDetectorWebApi.Controllers.Communication;
using WebPush;

namespace MotionDetectorWebApi.Services
{
    public class WebPushService : IWebPushService
    {
        private readonly IWebPushRepository _repository;
        private readonly WebPushClient _pushClient;
        private readonly IConfiguration _configuration;

        public WebPushService(IWebPushRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _pushClient = new WebPushClient();
            _configuration = configuration;
        }

        public VapidDetails GetVapidDetails(int userId)
        {
            return new VapidDetails(
                "mailto:abc.123@xyz.com",
                _configuration["WebPush:VapidKeyPublic"],
                _configuration["WebPush:VapidKeyPrivate"]
            );
        }

        public async Task<UserPushSubscription> Create(int userId, string endpoint, string p256dh, string auth)
        {
            var userPushSubscription = new UserPushSubscription
            {
                UserId = userId,
                EndPoint = endpoint,
                P256Dh = p256dh,
                Auth = auth
            };

            var exists = await _repository.Find(userId);
            if (exists == null)
            {
                await _repository.Create(userPushSubscription);
                return userPushSubscription;
            }

            exists.EndPoint = userPushSubscription.EndPoint;
            exists.P256Dh = userPushSubscription.P256Dh;
            exists.Auth = userPushSubscription.Auth;
            exists.UserId = userPushSubscription.UserId;

            return await _repository.Update(exists);
        }

        public async Task<PushMessage> SendNotificationForUser(int userId, string payload)
        {
            var subscription = await _repository.Find(userId);

            var webPushSubscription = new PushSubscription(
                subscription.EndPoint,
                subscription.P256Dh,
                subscription.Auth);

            var vapiDetails = GetVapidDetails(userId);

            var message = new PushMessage
            {
                Payload = payload,
                Date = DateTime.Now
            };

            _pushClient.SendNotification(webPushSubscription, message.ToJson(), vapiDetails);
            return message;
        }
    }
}