using System;
using System.Windows;
using System.Windows.Threading;

namespace DependencyScanner.ViewModel.Tools
{
    public static class DispatcherTools
    {
        public static void DispacherInvoke(Action callback) => Application.Current.Dispatcher.Invoke(callback);
    }
}
