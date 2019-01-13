using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Interfaces;
using GalaSoft.MvvmLight;

namespace DependencyScanner.Standalone.Setting
{
    public abstract class ObservableSettingsBase : ObservableObject, ISettings
    {
        public abstract string Id { get; }
    }
}
