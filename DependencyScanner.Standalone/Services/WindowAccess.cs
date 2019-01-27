using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Gui.Interfaces;
using System.Windows;

namespace DependencyScanner.Standalone.Services
{
    public class WindowAccess : IWindowAccess, IService
    {
        public Window MainWindow => App.Current.MainWindow;
    }
}
