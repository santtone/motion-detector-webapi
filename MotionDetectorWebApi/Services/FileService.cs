using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotionDetectorWebApi.Models;

namespace MotionDetectorWebApi.Services
{
    public class FileService : IFileService
    {
        private readonly IDriveService _driveService;

        public FileService(IDriveService driveService)
        {
            _driveService = driveService;
        }

        public async Task<List<MotionFile>> FindDriveFiles()
        {
            var driveFiles = await _driveService.GetFiles();
            var files = driveFiles.Select(f => new MotionFile
            {
                Id = f.Id,
                Name = f.Name,
                Date = f.CreatedTime,
                Link = f.WebContentLink,
                ThumbnailLink = f.ThumbnailLink,
                MimeType = f.MimeType
            }).ToList();
            files.RemoveAll(f => f.Link == null);
            return files;
        }

        public async Task DeleteAllDriveFiles()
        {
            await _driveService.DeleteAllFiles();
        }
    }
}