using System;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.Components
{
    public class SettingsViewHelper
    {
        public string PluginName { get; set; }
        public UserControl View { get; }

        public SettingsViewHelper(string pluginName, UserControl view)
        {
            PluginName = pluginName ?? throw new ArgumentNullException(nameof(pluginName));
            View = view ?? throw new ArgumentNullException(nameof(view));
        }
    }
}
