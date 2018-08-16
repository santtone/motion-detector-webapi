using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MotionDetectorWebApi.Services
{
    public class FileWatcher : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IDriveService _driveService;

        private readonly FileSystemWatcher _fileSystemWatcher;


        public FileWatcher(ILoggerFactory loggerFactory, IConfiguration configuration, IDriveService driveService)
        {
            _logger = loggerFactory.CreateLogger(typeof(FileWatcher));
            _fileSystemWatcher = new FileSystemWatcher(configuration["FilePath"]);
            _driveService = driveService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _fileSystemWatcher.Created += OnFileCreated;
            _fileSystemWatcher.EnableRaisingEvents = true;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _fileSystemWatcher.Created -= OnFileCreated;
            _fileSystemWatcher.EnableRaisingEvents = false;
            return Task.CompletedTask;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            _logger.LogDebug($"File created. {e}");
            _driveService.UploadFile(e.FullPath);
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            _logger.LogDebug($"File renamed. {e}");
        }

        private void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            _logger.LogDebug($"File deleted. {e}");
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            _logger.LogDebug($"File changed. {e}");
        }
    }
}