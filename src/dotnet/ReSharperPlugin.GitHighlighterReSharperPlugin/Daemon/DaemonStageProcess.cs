using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    public class DaemonStageProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess _process;
        private readonly ICSharpFile _file;
        private readonly List<string> _recentCommits;

        public DaemonStageProcess(IDaemonProcess process, ICSharpFile file, List<string> recentCommits)
        {
            _process = process;
            _file = file;
            _recentCommits = recentCommits;
        }

        public void Execute(Action<DaemonStageResult> committer)
        {
            var highlightings = new List<HighlightingInfo>();

            foreach (var commit in _recentCommits)
            {
                var processor = new ElementProcessor(highlightings, commit);
                _file.ProcessDescendants(processor);
            }
            
            committer(new DaemonStageResult(highlightings));
        }

        public IDaemonProcess DaemonProcess => _process;
    }
}