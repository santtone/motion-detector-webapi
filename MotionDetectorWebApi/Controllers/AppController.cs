using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MotionDetectorWebApi.Controllers
{
    [Route("api/app")]
    public class AppController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult("It works!");
        }
    }
}