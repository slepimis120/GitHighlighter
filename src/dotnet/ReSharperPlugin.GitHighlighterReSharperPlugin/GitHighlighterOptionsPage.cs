using JetBrains.Annotations;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionPages;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.IDE.UI.Options;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Feature.Services.Resources;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    [OptionsPage(Pid, "GitHighlighter", typeof(FeaturesEnvironmentOptionsThemedIcons.CodeInspections), ParentId = ToolsPage.PID)]
    public class GitHighlighterOptionsPage : BeSimpleOptionsPage
    {
        private const string Pid = "GitHighlighter";

        public GitHighlighterOptionsPage(Lifetime lifetime, OptionsPageContext optionsPageContext, [NotNull] OptionsSettingsSmartContext optionsSettingsSmartContext)
            : base(lifetime, optionsPageContext, optionsSettingsSmartContext)
        {
            AddIntOption((GitHighlighterSettings key) => key.NumberOfCommitsToHighlight, "Number of commits to highlight");
        }
    }
}