using System;

namespace DependencyScanner.Api.Interfaces
{
    public interface ISettingsManager
    {
        object Load(string fileName, Type settingsType);

        T Load<T>(string fileName) where T : ISettings, new();

        void Save<T>(T settings, string fileName) where T : ISettings;

        void Save(ISettings settings);
    }
}
