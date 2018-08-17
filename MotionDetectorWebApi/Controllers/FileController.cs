using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotionDetectorWebApi.Services;

namespace MotionDetectorWebApi.Controllers
{

    /// <summary>
    /// File API
    /// </summary>
    /// <remarks>Manage local and Google Drive files</remarks>
    [Route("api/files")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Get files
        /// </summary>
        /// <remarks>Get all files from Google Drive</remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var files = await _fileService.FindDriveFiles();
            return new JsonResult(files);
        }


        /// <summary>
        /// Delete files
        /// </summary>
        /// <remarks>Delete all Google Drive files</remarks>
        [Route("drive/all")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDriveFiles()
        {
            await _fileService.DeleteAllDriveFiles();
            return new AcceptedResult();
        }
    }
}