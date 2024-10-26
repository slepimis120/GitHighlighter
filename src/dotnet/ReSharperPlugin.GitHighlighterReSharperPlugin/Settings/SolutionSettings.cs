using JetBrains.Application.Settings;
using JetBrains.Application.Settings.WellKnownRootKeys;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    [SettingsKey(typeof(EnvironmentSettings), "Git Highlighter Settings")]
    public class SolutionSettings
    {
        [SettingsEntry(5, "Number of commits to highlight")]
        public int NumberOfCommitsToHighlight;
    }
}