//using DependencyScanner.Core.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DependencyScanner.Standalone.Setting
//{
//    internal static class SettingsLifetimetimeHandler
//    {
//        internal static void SaveSettings(ISettings a, ISettingsManager settingsManager)
//        {
//            var iPluginType = a.GetType().GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IPlugin<>));

//            if (iPluginType != null)
//            {
//                settingsManager.Save(settings, settings.CollectionKey, settings);
//            }
//        }

//        internal static void ReadSettings(ISettings a, ISettingsManager settingsManager)
//        {
//            var iPluginType = a.Instance.GetType().GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IPlugin<>));

//            if (iPluginType != null)
//            {
//                var settingsType = iPluginType.GetGenericArguments()[0];

//                var plugin = a.Instance as IPlugin<ISettings>;

//                var settings = (ISettings)(settingsManager.Load(plugin.CollectionKey, settingsType));

//                plugin.SetSettings(settings);
//            }
//        }
//    }
//}
