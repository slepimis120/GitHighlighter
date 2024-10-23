using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    [SolutionComponent]
    public class GitHighlighterSolutionComponent
    {
        private readonly GitHighlighterHelper _gitHelper;
        private readonly ISettingsStore _settingsStore;

        public GitHighlighterSolutionComponent(ISolution solution, ISettingsStore settingsStore)
        {
            _gitHelper = new GitHighlighterHelper(solution.SolutionFilePath.Directory.FullPath);
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
                    int numberOfCommitsToHighlight = settings.GetValue((GitHighlighterSettings s) => s.NumberOfCommitsToHighlight);
                    
                    List<Commit> recentCommits = _gitHelper.GetRecentCommits(numberOfCommitsToHighlight);
                    
                }
            });
        }
    }
}