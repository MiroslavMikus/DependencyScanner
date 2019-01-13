using DependencyScanner.Api.Interfaces;
using DependencyScanner.Standalone.Setting;
using GalaSoft.MvvmLight;
using System.Windows;

namespace DependencyScanner.Standalone.Components
{
    public class MainSettings : ObservableObject, ISettings
    {
        public string Id { get; } = "MainViewSettings";

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

        private bool _showCmdButton = true;

        public bool ShowCmdButton
        {
            get => _showCmdButton;
            set => Set(ref _showCmdButton, value);
        }

        private bool _showFolderButton = true;

        public bool ShowFolderButton
        {
            get => _showFolderButton;
            set => Set(ref _showFolderButton, value);
        }

        private bool _showOpenButton = true;

        public bool ShowOpenButton
        {
            get => _showOpenButton;
            set => Set(ref _showOpenButton, value);
        }

        private bool _showOpenUrlButton = true;

        public bool ShowOpenUrlButton
        {
            get => _showOpenUrlButton;
            set => Set(ref _showOpenUrlButton, value);
        }

        private bool _showOpenFileButton = true;

        public bool ShowOpenFileButton
        {
            get => _showOpenFileButton;
            set => Set(ref _showOpenFileButton, value);
        }
    }
}
