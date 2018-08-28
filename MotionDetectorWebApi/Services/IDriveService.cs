using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;

namespace MotionDetectorWebApi.Services
{
    public interface IDriveService
    {
        Task<List<File>> GetFiles();
        Task<File> UploadFile(string filePath);
        Task DeleteAllFiles();
    }
}