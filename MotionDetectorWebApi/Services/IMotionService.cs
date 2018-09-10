using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotionConfigManager;

namespace MotionDetectorWebApi.Services
{
    public interface IMotionService
    {
        Task<MotionConfig> GetConfig();
        Task<MotionConfig> UpdateConfig(MotionConfig config);
    }
}