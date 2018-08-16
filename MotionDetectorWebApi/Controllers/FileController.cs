using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotionDetectorWebApi.Services;

namespace MotionDetectorWebApi.Controllers
{
    [Route("api/files")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var files = await _fileService.FindDriveFiles();
            return new JsonResult(files);
        }
    }
}