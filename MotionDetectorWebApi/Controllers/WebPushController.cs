using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotionDetectorWebApi.Controllers.Communication;
using MotionDetectorWebApi.Services;

namespace MotionDetectorWebApi.Controllers
{
    [Route("api/webpush")]
    public class WebPushController : Controller
    {
        private readonly IWebPushService _webPushService;

        public WebPushController(IWebPushService webPushService)
        {
            _webPushService = webPushService;
        }

        [Route("vapid")]
        [HttpGet]
        public IActionResult Get()
        {
            var vapid = _webPushService.GetVapidDetails(1);
            return new JsonResult(vapid);
        }

        [Route("subscriptions")]
        [HttpPost]
        public IActionResult Post([FromBody] PushSubscriptionRequest subscriptionRequest)
        {
            _webPushService.Create(1, subscriptionRequest.Endpoint, subscriptionRequest.P256dh,
                subscriptionRequest.Auth);
            return new OkResult();
        }

        [Route("messages")]
        [HttpPost("messages")]
        public async Task<IActionResult> Post([FromBody] PushRequest pushRequest)
        {
            pushRequest.Message = pushRequest.Message;
            var message = await _webPushService.SendNotificationForUser(1, pushRequest.Message);
            return new CreatedResult("", message);
        }
    }
}