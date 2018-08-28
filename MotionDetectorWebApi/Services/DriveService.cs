using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;
using GoogleDriveClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MotionDetectorWebApi.Services
{
    public class DriveService : IDriveService
    {
        private readonly DriveClient _driveClient;
        private readonly ILogger _logger;


        public DriveService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(DriveService));

            _driveClient = new DriveClient(
                configuration["GoogleDrive:AppServiceEmail"],
                configuration["GoogleDrive:KeyFilePath"],
                configuration["GoogleDrive:KeyPassword"]);
        }

        public Task<List<File>> GetFiles()
        {
            return _driveClient.ListFiles();
        }

        public async Task<File> UploadFile(string filePath)
        {
            var file = await _driveClient.UploadFile(filePath);
            _logger.LogDebug($"File uploaded to Google Drive. Id={file.Id}");
            return file;
        }

        public async Task DeleteAllFiles()
        {
            await _driveClient.DeleteAllFiles();
        }
    }
}