using DependencyScanner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Standalone.Setting
{
    internal static class SettingsLifetimetimeHandler
    {
        internal static void SaveSettings(object a, ISettingsManager settingsManager)
        {
            var iPluginType = a.GetType().GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IPlugin<>));

            if (iPluginType != null)
            {
                var settingsType = iPluginType.GetGenericArguments()[0];

                var plugin = a as IPlugin<ISettings>;

                settingsManager.Save(plugin.Settings, plugin.CollectionKey, settingsType);
            }
        }

        internal static void ReadSettings(Autofac.Core.IActivatingEventArgs<object> a, ISettingsManager settingsManager)
        {
            var iPluginType = a.Instance.GetType().GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IPlugin<>));

            if (iPluginType != null)
            {
                var settingsType = iPluginType.GetGenericArguments()[0];

                var plugin = a.Instance as IPlugin<ISettings>;

                var settings = (ISettings)(settingsManager.Load(plugin.CollectionKey, settingsType));

                plugin.SetSettings(settings);
            }
        }
    }
}
