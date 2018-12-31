namespace DependencyScanner.Standalone.Setting
{
    public interface ISettingsManager
    {
        T Load<T>(string collectionKey) where T : ISettings, new();

        void Save<T>(T settings, string collectionKey) where T : ISettings;
    }
}
