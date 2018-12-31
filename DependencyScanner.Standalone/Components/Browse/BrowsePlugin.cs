using DependencyScanner.Core.Interfaces;
using DependencyScanner.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DependencyScanner.Standalone.Components.Browse
{
    public abstract class BaseTest<T> : IPlugin<T> where T : ISettings
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

    public class BrowsePlugin : BaseTest<SomeTest>
    {
        public override string Title => "Browse";

        public override string Description => "Scan and browse your solutions.";

        public override string CollectionKey { get; } = "BrowseSettings";

        public BrowsePlugin(BrowseViewModel browseViewModel)
        {
            Order = 1;

            ContentView = new BrowseView
            {
                DataContext = browseViewModel
            };
        }
    }
}
