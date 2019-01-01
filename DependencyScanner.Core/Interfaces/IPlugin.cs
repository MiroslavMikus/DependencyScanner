using System;
using System.Windows.Controls;

namespace DependencyScanner.Core.Interfaces
{
    public interface IPlugin
    {
        string Title { get; }
        string Description { get; }
        UserControl ContentView { get; }
        int Order { get; }

        //Page Help { get; }
    }

    public interface IPlugin<out T> : IPlugin where T : ISettings
    {
        string CollectionKey { get; }

        T Settings { get; }

        void SetSettings(ISettings settings);

        UserControl SettingsView { get; }
    }
}
