using LiteDB;

namespace DependencyScanner.Standalone.Setting
{
    public class SettingsManager : ISettingsManager
    {
        public const string DefaultKey = "default";

        private readonly LiteDatabase _database;

        public SettingsManager(LiteDatabase database)
        {
            _database = database;
        }

        public T Load<T>(string collectionKey) where T : ISettings, new()
        {
            var collection = _database.GetCollection<T>(collectionKey);

            var result = collection.FindById(DefaultKey);

            return result == null ? new T() : result;
        }

        public void Save<T>(T settings, string collectionKey) where T : ISettings
        {
            var collection = _database.GetCollection<T>(collectionKey);

            collection.Upsert(settings);
        }
    }
}
