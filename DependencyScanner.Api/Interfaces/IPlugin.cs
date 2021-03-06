﻿using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DependencyScanner.Api.Interfaces
{
    public interface IPlugin
    {
        string Title { get; }
        string Description { get; }
        UserControl ContentView { get; }
        int Order { get; }

        void OnStarted();

        //Page Help { get; }
    }

    public interface IPlugin<out T> : IPlugin where T : ISettings
    {
        T Settings { get; }

        UserControl SettingsView { get; }
    }
}
