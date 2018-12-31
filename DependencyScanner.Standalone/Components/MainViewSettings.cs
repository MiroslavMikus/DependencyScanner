using DependencyScanner.Standalone.Setting;
using System.Windows;

namespace DependencyScanner.Standalone.Components
{
    public class MainViewSettings : ObservableSettingsBase
    {
        private double _windowLeft = 0;

        public double Window_Left
        {
            get => _windowLeft;
            set
            {
                Set(ref _windowLeft, value);
            }
        }

        private double _windowTop = 0;

        public double Window_Top
        {
            get => _windowTop;
            set
            {
                Set(ref _windowTop, value);
            }
        }

        private WindowState _windowState = WindowState.Maximized;

        public WindowState Window_State
        {
            get => _windowState;
            set
            {
                Set(ref _windowState, value);
            }
        }

        private double _windowsWidth = 1000;

        public double WindowWidth
        {
            get => _windowsWidth;
            set
            {
                Set(ref _windowsWidth, value);
            }
        }

        private double _windowsHeight = 500;

        public double WindowHeight
        {
            get => _windowsHeight;
            set
            {
                Set(ref _windowsHeight, value);
            }
        }
    }
}
