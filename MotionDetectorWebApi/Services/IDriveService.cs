using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;

namespace MotionDetectorWebApi.Services
{
    public interface IDriveService
    {
        Task<List<File>> GetFiles();
        Task UploadFile(string filePath);
    }
}