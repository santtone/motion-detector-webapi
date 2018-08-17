using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MotionDetectorWebApi.Controllers
{
    /// <summary>
    /// App API
    /// </summary>
    /// <remarks>API for application information</remarks>
    [Route("api/app")]
    public class AppController
    {
        /// <summary>
        /// Get Info
        /// </summary>
        /// <remarks>Get application information</remarks>
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult("It works!");
        }
    }
}