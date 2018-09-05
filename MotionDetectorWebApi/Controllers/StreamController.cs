using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotionDetectorWebApi.Services;

namespace MotionDetectorWebApi.Controllers
{
    [Authorize]
    [Route("api/stream")]
    public class StreamController : Controller
    {
        private readonly IStreamTokenService _streamTokenService;

        public StreamController(IStreamTokenService streamTokenService)
        {
            _streamTokenService = streamTokenService;
        }

        [Route("token")]
        [HttpGet]
        public IActionResult Get()
        {
            var token = _streamTokenService.GetToken();
            return new JsonResult(token);
        }
    }
}