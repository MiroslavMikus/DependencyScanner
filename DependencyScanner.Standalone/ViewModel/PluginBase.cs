using DependencyScanner.Core.Interfaces;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.Components.Browse
{
    public abstract class PluginBase<T> : IPlugin<T> where T : ISettings
    {
        private ISettings _settings;

        public abstract string CollectionKey { get; }

        public T Settings => (T)_settings;

        public abstract string Title { get; }

        public abstract string Description { get; }

        public virtual UserControl ContentView { get; protected set; }

        public virtual int Order { get; protected set; }

        public virtual void SetSettings(ISettings settings)
        {
            _settings = settings;
        }
    }
}
