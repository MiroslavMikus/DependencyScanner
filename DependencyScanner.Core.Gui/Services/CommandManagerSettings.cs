using DependencyScanner.Api.Interfaces;
using GalaSoft.MvvmLight;

namespace DependencyScanner.Core.Gui.Services
{
    public class CommandManagerSettings : ObservableObject, ISettings
    {
        public string Id => "CommandManagerSettings";

        private string _consoleTool = "cmd.exe";

        public string ConsoleTool
        {
            get { return _consoleTool; }
            set { Set(ref _consoleTool, value); }
        }

        private string _webBrowser;

        public string WebBrowser
        {
            get { return _webBrowser; }
            set { Set(ref _webBrowser, value); }
        }

        private string _textEditor = "notepad";

        public string TextEditor
        {
            get { return _textEditor; }
            set { Set(ref _textEditor, value); }
        }
    }
}
