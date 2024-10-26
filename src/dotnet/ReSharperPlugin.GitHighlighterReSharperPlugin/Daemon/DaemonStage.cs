using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.CSharp.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    [DaemonStage]
    public class DaemonStage : CSharpDaemonStageBase
    {
        private readonly SolutionComponent _solutionComponent;

        public DaemonStage(SolutionComponent solutionComponent)
        {
            _solutionComponent = solutionComponent;
        }

        protected override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, ICSharpFile file)
        {
            var recentCommitsTask = _solutionComponent.CommitsLoaded;
            recentCommitsTask.Wait();

            if (recentCommitsTask.Result)
            {
                var numberOfCommitsToHighlight = settings.GetValue((SolutionSettings s) => s.NumberOfCommitsToHighlight);
                var modifiedFiles = _solutionComponent.GetModifiedFilesFromRecentCommits(numberOfCommitsToHighlight);

                if (modifiedFiles.Contains(file.GetSourceFile().GetLocation().FullPath))
                {
                    var recentCommits = _solutionComponent.GetRecentCommits(numberOfCommitsToHighlight);
                    return new DaemonStageProcess(process, file, recentCommits);
                }
            }

            return new DaemonStageProcess(process, file, new List<string>());
        }
    }
}