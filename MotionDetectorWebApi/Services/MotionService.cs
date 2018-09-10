using System.Threading.Tasks;
using MotionConfigManager;

namespace MotionDetectorWebApi.Services
{
    public class MotionService : IMotionService
    {
        private readonly MotionConfigHandler _motionConfigHandler;
        private readonly string _motionConfigPath;

        public MotionService()
        {
            _motionConfigPath = "C:\\dev-private\\motion-detector\\app_data\\motion.conf";
            _motionConfigHandler =
                new MotionConfigHandler("http://192.168.100.12:8080/0", _motionConfigPath);
        }

        public async Task<MotionConfig> GetConfig()
        {
            return await _motionConfigHandler.ReadConfiguration(_motionConfigPath);
        }

        public async Task<MotionConfig> UpdateConfig(MotionConfig config)
        {
            await _motionConfigHandler.SaveConfiguration(config);
            return await GetConfig();
        }
    }
}