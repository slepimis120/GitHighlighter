using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.CSharp;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    [StaticSeverityHighlighting(
        Severity.WARNING,
        typeof(HighlightingGroupIds.CodeSmellStatic))]
    public class GitCommitHighlighting : IHighlighting
    {
        internal const string SeverityId = "GitCommitHighlighting";
        private readonly DocumentRange _range;
        private readonly string _toolTip;

        public GitCommitHighlighting(DocumentRange range, string tooltip)
        {
            _range = range;
            _toolTip = tooltip;
        }

        public bool IsValid() => _range.IsValid();

        public DocumentRange CalculateRange() => _range;

        public string ToolTip => _toolTip;

        public string ErrorStripeToolTip => _toolTip;
    }
}