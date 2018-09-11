using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotionConfigManager;
using MotionDetectorWebApi.Services;

namespace MotionDetectorWebApi.Controllers
{
    [Authorize]
    [Route("api/motion")]
    public class MotionController : Controller
    {
        private readonly IMotionService _motionService;

        public MotionController(IMotionService motionService)
        {
            _motionService = motionService;
        }

        [Route("restart")]
        [HttpGet]
        public async Task<IActionResult> Restart()
        {
            await _motionService.Restart();
            return new OkResult();
        }

        [Route("config")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var config = await _motionService.GetConfig();
            return new JsonResult(config);
        }

        [Route("config")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MotionConfig config)
        {
            await _motionService.UpdateConfig(config);
            return new OkResult();
        }
    }
}