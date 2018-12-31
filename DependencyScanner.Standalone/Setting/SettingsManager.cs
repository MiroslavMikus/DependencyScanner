using LiteDB;

namespace DependencyScanner.Standalone.Setting
{
    public class SettingsManager : ISettingsManager
    {
        private readonly LiteDatabase _database;

        public SettingsManager(LiteDatabase database)
        {
            _database = database;
        }

        public T Load<T>(string collectionKey) where T : ISettings, new()
        {
            var collection = _database.GetCollection<T>(collectionKey);

            var result = collection.FindById("default");

            return result == null ? new T() : result;
        }

        public void Save<T>(T settings) where T : ISettings
        {
            var collection = _database.GetCollection<T>(settings.CollectionKey);

            collection.Upsert(settings);
        }
    }
}
