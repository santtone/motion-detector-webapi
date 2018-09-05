using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MotionDetectorWebApi.Models;
using MotionDetectorWebApi.Utils;

namespace MotionDetectorWebApi.Controllers
{
    [Authorize]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var user = new User
            {
                Id = 0,
                Username = AuthHelper.UsernameFromIdentity(_configuration["Azure:AD:UsernamePostfix"], User.Identity)
            };
            return new JsonResult(user);
        }
    }
}