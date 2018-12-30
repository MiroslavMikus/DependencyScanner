using System.Windows.Controls;

namespace DependencyScanner.Core.Interfaces
{
    public interface IPlugin
    {
        string Title { get; }
        string Description { get; }
        UserControl ContentView { get; }
        int Order { get; }
        //Page Settings { get; }
        //Page Help { get; }
    }
}
