using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Interfaces;
using LiteDB;
using System;
using System.Reflection;

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

        /// <summary>
        /// Reads settings from the lite database.
        /// </summary>
        /// <param name="collectionKey"></param>
        /// <param name="settingsType">Providet type has to have an empty constructor!</param>
        /// <returns></returns>
        public object Load(string collectionKey, Type settingsType)
        {
            var collection = _database.GetCollection(collectionKey);

            var result = collection.FindById(DefaultKey);

            return result == null ? Activator.CreateInstance(settingsType) : BsonMapper.Global.ToObject(settingsType, result);
        }

        public void Save<T>(T settings, string collectionKey) where T : ISettings
        {
            var collection = _database.GetCollection<T>(collectionKey);

            collection.Upsert(settings);
        }

        public void Save(ISettings settings)
        {
            var collection = _database.GetCollection(settings.CollectionKey);

            var document = BsonMapper.Global.ToDocument(settings.GetType(), settings);

            collection.Upsert(document);
        }
    }
}
