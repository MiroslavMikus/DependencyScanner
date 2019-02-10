using DependencyScanner.Api.Interfaces;
using System.Windows.Controls;

namespace DependencyScanner.Core.Gui.ViewModel
{
    public abstract class PluginBase<T> : IPlugin<T> where T : ISettings
    {
        public T Settings { get; }

        public abstract string Title { get; }

        public abstract string Description { get; }

        public virtual UserControl ContentView { get; protected set; }
        public virtual UserControl SettingsView { get; protected set; }

        public virtual int Order { get; protected set; }

        public PluginBase(T settings)
        {
            Settings = settings;
        }
    }
}
