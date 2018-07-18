using System;
using System.Diagnostics;
using System.IO;

namespace MotionFileWatcher
{
    public class FileWatcher
    {
        private readonly FileSystemWatcher _fileSystemWatcher;
        public event EventHandler FilesChanged;

        public FileWatcher(string path)
        {
            _fileSystemWatcher = new FileSystemWatcher(path);

            _fileSystemWatcher.Created += FileCreated;
            _fileSystemWatcher.Changed += FileChanged;
            _fileSystemWatcher.Deleted += FileDeleted;
            _fileSystemWatcher.Renamed += FileRenamed;
        }

        public void Start()
        {
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
        }

        private void FileRenamed(object sender, RenamedEventArgs e)
        {
            Debug.WriteLine($"File renamed - {e.Name}");
            FilesChanged?.Invoke(this, e);
        }

        private void FileDeleted(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine($"File deleted - {e.Name}");
            FilesChanged?.Invoke(this, e);
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine($"File changed - {e.Name}");
            FilesChanged?.Invoke(this, e);
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine($"File created - {e.Name}");
            FilesChanged?.Invoke(this, e);
        }
    }
}