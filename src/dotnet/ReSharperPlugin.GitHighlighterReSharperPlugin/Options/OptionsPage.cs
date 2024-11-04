using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionPages;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.IDE.UI.Options;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.DataFlow;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    [OptionsPage(Pid, "GitHighlighter", typeof(FeaturesEnvironmentOptionsThemedIcons.CodeInspections), ParentId = ToolsPage.PID)]
    public class OptionsPage : BeSimpleOptionsPage
    {
        private const string Pid = "GitHighlighter";

        public OptionsPage(
            Lifetime lifetime,
            OptionsPageContext optionsPageContext,
            [NotNull] OptionsSettingsSmartContext optionsSettingsSmartContext)
            : base(lifetime, optionsPageContext, optionsSettingsSmartContext)
        {
            var numberOfCommitsProperty = new Property<int>("NumberOfCommitsToHighlight");
            optionsSettingsSmartContext.SetBinding(lifetime, (SolutionSettings key) => key.NumberOfCommitsToHighlight, numberOfCommitsProperty);

            AddIntOption((SolutionSettings key) => key.NumberOfCommitsToHighlight, "Number of commits to highlight");

            numberOfCommitsProperty.Change.Advise(lifetime, _ =>
            {
                SettingsChangeNotifier.NotifyCommitsSettingChanged();
            });
        }

    }
}