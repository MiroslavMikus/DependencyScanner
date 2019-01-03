//using DependencyScanner.Core.Interfaces;
//using DependencyScanner.Standalone.Setting;
//using GalaSoft.MvvmLight;
//using System;

//namespace DependencyScanner.Standalone.ViewModel
//{
//    public class SettingsViewModel<T> : ViewModelBase, IDisposable where T : ISettings, new()
//    {
//        private readonly ISettingsManager _settingsManager;
//        private readonly string _settingsCollectionName;

//        private T _settings;

//        public T Settings
//        {
//            get { return _settings != null ? _settings : (_settings = _settingsManager.Load<T>(_settingsCollectionName)); ; }
//            set { Set(ref _settings, value); }
//        }

//        public SettingsViewModel(ISettingsManager settingsManager)
//        {
//            _settingsManager = settingsManager;
//            _settingsCollectionName = settingsCollectionName;
//        }

//        public void Dispose()
//        {
//            _settingsManager.Save(Settings, _settingsCollectionName);

//            OnDispose();
//        }

//        protected virtual void OnDispose()
//        {
//        }
//    }
//}
