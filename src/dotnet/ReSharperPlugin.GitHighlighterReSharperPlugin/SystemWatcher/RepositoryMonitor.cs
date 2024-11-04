using System;
using System.IO;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    public class RepositoryMonitor
    {
        private readonly FileSystemWatcher _watcher;
        private readonly Lazy<IDaemon> _daemon;
        private readonly Helper _helper;

        public RepositoryMonitor(string repositoryPath, Lazy<IDaemon> daemon, Helper helper)
        {
            _daemon = daemon;
            _helper = helper;

            _watcher = new FileSystemWatcher
            {
                Path = Path.Combine(repositoryPath, ".git"),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*",
                IncludeSubdirectories = true
            };

            _watcher.Changed += OnGitRepositoryChanged;
            _watcher.Created += OnGitRepositoryChanged;
            _watcher.Deleted += OnGitRepositoryChanged;
            _watcher.Renamed += OnGitRepositoryChanged;
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
        }

        private void OnGitRepositoryChanged(object sender, FileSystemEventArgs e)
        {
            _helper.InvalidateCachedCommits();
            _daemon.Value.Invalidate("Commits setting changed");
        }
    }
}