using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.DocumentModel;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    [ConfigurableSeverityHighlighting("GitCommitHighlighting", "CSharp", OverlapResolve = OverlapResolveKind.NONE, ToolTipFormatString = "Highlighted by Git commit")]
    public class GitCommitHighlighting : IHighlighting
    {
        private readonly DocumentRange _range;
        private readonly string _toolTip;

        public GitCommitHighlighting(DocumentRange range, string tooltip)
        {
            _range = range;
            _toolTip = tooltip;
        }

        public bool IsValid() => true;

        public DocumentRange CalculateRange() => _range;

        public string ToolTip => _toolTip;

        public string ErrorStripeToolTip => _toolTip;
    }
}