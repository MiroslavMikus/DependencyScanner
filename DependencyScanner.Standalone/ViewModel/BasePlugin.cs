using DependencyScanner.Core.Interfaces;
using DependencyScanner.Standalone.Setting;
using GalaSoft.MvvmLight;
using System;

namespace DependencyScanner.Standalone.ViewModel
{
    public class BasePlugin<T> : ViewModelBase, IDisposable where T : ISettings, new()
    {
        private T _settings;
        private readonly ISettingsManager _settingsManager;
        private readonly string _settingsCollectionName;

        public T Settings
        {
            get { return _settings != null ? _settings : (_settings = _settingsManager.Load<T>(_settingsCollectionName)); ; }
            set { Set(ref _settings, value); }
        }

        public BasePlugin(ISettingsManager settingsManager, string settingsCollectionName)
        {
            _settingsManager = settingsManager;
            _settingsCollectionName = settingsCollectionName;
        }

        public void Dispose()
        {
            _settingsManager.Save(Settings, _settingsCollectionName);

            OnDispose();
        }

        protected virtual void OnDispose()
        {
        }
    }
}
