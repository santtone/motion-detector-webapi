using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MotionConfigManager;

namespace MotionDetectorWebApi.Services
{
    public class MotionService : IMotionService
    {
        private readonly MotionConfigHandler _motionConfigHandler;
        private readonly string _motionConfigPath;

        public MotionService(IConfiguration configuration)
        {
            _motionConfigPath = configuration["Motion:ConfigPath"];
            _motionConfigHandler =
                new MotionConfigHandler(configuration["Motion:WebControl"], _motionConfigPath);
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

        public async Task Restart()
        {
            await _motionConfigHandler.RestartMotion();
        }
    }
}