using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionPages;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.IDE.UI.Options;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.DataFlow;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    [OptionsPage(Pid, "GitHighlighter", typeof(FeaturesEnvironmentOptionsThemedIcons.CodeInspections), ParentId = ToolsPage.PID)]
    public class OptionsPage : BeSimpleOptionsPage
    {
        private const string Pid = "GitHighlighter";
        private readonly IDaemon _daemon;
        private readonly Helper _helper;

        public OptionsPage(Lifetime lifetime, OptionsPageContext optionsPageContext, [NotNull] OptionsSettingsSmartContext optionsSettingsSmartContext, IDaemon daemon, Helper helper)
            : base(lifetime, optionsPageContext, optionsSettingsSmartContext)
        {
            _daemon = daemon;
            _helper = helper;

            var numberOfCommitsProperty = new Property<int>("NumberOfCommitsToHighlight");
            optionsSettingsSmartContext.SetBinding(lifetime, (SolutionSettings key) => key.NumberOfCommitsToHighlight, numberOfCommitsProperty);

            AddIntOption((SolutionSettings key) => key.NumberOfCommitsToHighlight, "Number of commits to highlight");

            numberOfCommitsProperty.Change.Advise(lifetime, _ =>
            {
                _helper.InvalidateCachedCommits();
                _daemon.Invalidate();
            });
        }
    }
}