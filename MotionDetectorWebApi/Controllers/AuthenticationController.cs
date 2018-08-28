using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotionDetectorWebApi.Controllers.Communication;
using MotionDetectorWebApi.Services;

namespace MotionDetectorWebApi.Controllers
{
    [Route("api/authenticate")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<ActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            var token = await _authenticationService.GetToken(request.Username, request.Password);
            return new JsonResult(token);
        }
    }
}