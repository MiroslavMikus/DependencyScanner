using DependencyScanner.Core.Interfaces;
using System;

namespace DependencyScanner.Standalone.Setting
{
    public interface ISettingsManager
    {
        object Load(string collectionKey, Type settingsType);

        T Load<T>(string collectionKey) where T : ISettings, new();

        void Save<T>(T settings, string collectionKey) where T : ISettings;

        void Save(ISettings settings);
    }
}
