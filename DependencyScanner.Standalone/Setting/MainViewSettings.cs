using System.Windows;

namespace DependencyScanner.Standalone.Setting
{
    public class MainViewSettings : SettingsBase
    {
        public override string CollectionKey => "MainViewSettings";

        private double _windowLeft;

        public double Window_Left
        {
            get => _windowLeft;
            set
            {
                Set(ref _windowLeft, value);
            }
        }

        private double _windowTop;

        public double Window_Top
        {
            get => _windowTop;
            set
            {
                if (Set(ref _windowTop, value))
                {
                }
            }
        }

        private WindowState _windowState;

        public WindowState Window_State
        {
            get => _windowState;
            set
            {
                Set(ref _windowState, value);
            }
        }

        private double _windowsWidth;

        public double WindowWidth
        {
            get => _windowsWidth;
            set
            {
                Set(ref _windowsWidth, value);
            }
        }

        private double _windowsHeight;

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
