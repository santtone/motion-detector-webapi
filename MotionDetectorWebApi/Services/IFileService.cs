using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotionDetectorWebApi.Models;

namespace MotionDetectorWebApi.Services
{
    public interface IFileService
    {
        Task<List<MotionFile>> FindDriveFiles();
    }
}