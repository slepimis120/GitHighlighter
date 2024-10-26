using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    [SolutionComponent]
    public class SolutionComponent
    {
        private readonly Helper _gitHelper;
        private readonly ISettingsStore _settingsStore;
        private readonly TaskCompletionSource<bool> _commitsLoaded = new TaskCompletionSource<bool>();
        private readonly ConcurrentBag<string> _recentCommits = new ConcurrentBag<string>();

        public SolutionComponent(ISolution solution, ISettingsStore settingsStore)
        {
            _gitHelper = new Helper(solution.SolutionFilePath.Directory.FullPath);
            _settingsStore = settingsStore;
            OnSolutionOpened();
        }

        public void OnSolutionOpened()
        {
            Task.Run(() =>
            {
                if (_gitHelper.IsGitRepository())
                {
                    var settings = _settingsStore.BindToContextTransient(ContextRange.ApplicationWide);
                    int numberOfCommitsToHighlight = settings.GetValue((SolutionSettings s) => s.NumberOfCommitsToHighlight);

                    foreach (var commit in _gitHelper.GetRecentCommits(numberOfCommitsToHighlight))
                    {
                        _recentCommits.Add(commit);
                    }

                    _commitsLoaded.SetResult(true);

                }
                else
                {
                    _commitsLoaded.SetResult(false);
                }
            });
        }

        public List<string> GetRecentCommits(int numberOfCommits)
        {
            return new List<string>(_recentCommits);
        }

        public Task<bool> CommitsLoaded => _commitsLoaded.Task;
    }
}