using DependencyScanner.Api.Interfaces;
using GalaSoft.MvvmLight.Threading;
using System.Windows.Forms;

namespace DependencyScanner.Standalone.Services
{
    public class FolderPicker : IFolderPicker
    {
        public string PickFolder()
        {
            string selectedFolder = string.Empty;

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    DialogResult result = dialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        selectedFolder = dialog.SelectedPath;
                    }
                }
            });

            return selectedFolder;
        }
    }
}
