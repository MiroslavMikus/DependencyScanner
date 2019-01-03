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
        T Settings { get; }

        UserControl SettingsView { get; }
    }
}
