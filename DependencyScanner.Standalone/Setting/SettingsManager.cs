using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Standalone.Components.Browse;
using LiteDB;
using System;
using System.Reflection;

namespace DependencyScanner.Standalone.Setting
{
    public class SettingsManager : ISettingsManager
    {
        private readonly LiteDatabase _database;

        public SettingsManager(LiteDatabase database)
        {
            _database = database;
        }

        public T Load<T>(string fileName) where T : ISettings, new()
        {
            var collection = _database.GetCollection<T>(fileName);

            var result = collection.FindById(fileName);

            return result == null ? new T() : result;
        }

        /// <summary>
        /// Reads settings from the lite database.
        /// </summary>
        /// <param name="collectionKey"></param>
        /// <param name="settingsType">Providet type has to have an empty constructor!</param>
        /// <returns></returns>
        public object Load(string fileName, Type settingsType)
        {
            var collection = _database.GetCollection(fileName);

            var result = collection.FindById(fileName);

            return result == null ? Activator.CreateInstance(settingsType) : BsonMapper.Global.ToObject(settingsType, result);
        }

        public void Save<T>(T settings, string fileName) where T : ISettings
        {
            var collection = _database.GetCollection<T>(fileName);

            collection.Upsert(settings);
        }

        public void Save(ISettings settings)
        {
            var collection = _database.GetCollection(settings.Id);

            var document = BsonMapper.Global.ToDocument(settings.GetType(), settings);

            collection.Upsert(document);
        }
    }
}
