using System;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    public static class SettingsChangeNotifier
    {
        public static event Action OnCommitsSettingChanged;

        public static void NotifyCommitsSettingChanged()
        {
            OnCommitsSettingChanged?.Invoke();
        }
    }
}