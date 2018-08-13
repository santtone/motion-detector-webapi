using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;
using GoogleDriveClient;
using Microsoft.Extensions.Configuration;

namespace MotionDetectorWebApi.Services
{
    public class DriveService : IDriveService
    {
        private readonly DriveClient _driveClient;

        public DriveService(IConfiguration configuration)
        {
            _driveClient = new DriveClient(
                configuration["GoogleDrive:AppServiceEmail"],
                configuration["GoogleDrive:KeyFilePath"],
                configuration["GoogleDrive:KeyPassword"]);
        }

        public Task<List<File>> GetFiles()
        {
            return _driveClient.ListFiles();
        }
    }
}