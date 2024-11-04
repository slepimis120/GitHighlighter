using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{ 
    [SolutionComponent]
    public class SolutionComponent
    {
        private readonly Helper _gitHelper;
        private readonly ISettingsStore _settingsStore;
        private readonly Lazy<IDaemon> _daemon;
        private ConcurrentBag<string> _recentCommits = new ConcurrentBag<string>();
        private readonly TaskCompletionSource<bool> _commitsLoaded = new TaskCompletionSource<bool>();
        private readonly RepositoryMonitor _repositoryMonitor;

        public Task<bool> CommitsLoaded => _commitsLoaded.Task;

        public SolutionComponent(ISolution solution, ISettingsStore settingsStore, Lazy<IDaemon> daemon)
        {
            if (solution == null)
                throw new ArgumentNullException(nameof(solution));

            _gitHelper = new Helper(solution.SolutionFilePath.Directory.FullPath);
            _settingsStore = settingsStore;
            _daemon = daemon;
            _repositoryMonitor = new RepositoryMonitor(solution.SolutionFilePath.Directory.FullPath, daemon, _gitHelper);
            _repositoryMonitor.Start();
            
            SettingsChangeNotifier.OnCommitsSettingChanged += OnCommitsSettingChanged;
            
            Task.Run(async () => await OnSolutionOpened()).Wait();
        }

        private async Task OnSolutionOpened()
        {
            if (_gitHelper.IsGitRepository())
            {
                var settings = _settingsStore.BindToContextTransient(ContextRange.ApplicationWide);
                int numberOfCommitsToHighlight = settings.GetValue((SolutionSettings s) => s.NumberOfCommitsToHighlight);
                
                var recentCommits = await _gitHelper.GetRecentCommitsAsync(numberOfCommitsToHighlight);
                foreach (var commit in recentCommits)
                {
                    _recentCommits.Add(commit);
                }
                _commitsLoaded.SetResult(true);
            }
            else
            {
                _commitsLoaded.SetResult(false);
            }
        }

        private void OnCommitsSettingChanged()
        {
            Task.Run(async () => await ReloadRecentCommits());
            _daemon.Value.Invalidate("Commits setting changed due to setting update");
        }

        private async Task ReloadRecentCommits()
        {
            var settings = _settingsStore.BindToContextTransient(ContextRange.ApplicationWide);
            int numberOfCommitsToHighlight = settings.GetValue((SolutionSettings s) => s.NumberOfCommitsToHighlight);

            var recentCommits = await _gitHelper.GetRecentCommitsAsync(numberOfCommitsToHighlight);

            var newRecentCommits = new ConcurrentBag<string>(recentCommits);
            
            _recentCommits = newRecentCommits;
            foreach (var commit in recentCommits)
            {
                _recentCommits.Add(commit);
            }
        }

        public async Task<List<string>> GetRecentCommitsAsync(int numberOfCommits)
        {
            return await Task.Run(() => _recentCommits.Take(numberOfCommits).ToList());
        }

        public async Task<List<string>> GetModifiedFilesFromRecentCommitsAsync(int numberOfCommits)
        {
            var modifiedFiles = new List<string>();

            if (numberOfCommits > 0)
            {
                var recentCommits = await GetRecentCommitsAsync(numberOfCommits);
                foreach (var commit in recentCommits)
                {
                    modifiedFiles.AddRange(_gitHelper.GetModifiedFiles(commit));
                }
            }

            return modifiedFiles.Distinct().ToList();
        }
    }
}
