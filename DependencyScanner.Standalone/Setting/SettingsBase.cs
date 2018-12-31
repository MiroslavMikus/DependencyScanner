using GalaSoft.MvvmLight;
using LiteDB;

namespace DependencyScanner.Standalone.Setting
{
    public abstract class SettingsBase : ObservableObject, ISettings
    {
        [BsonId]
        public string Id { get; } = "default";

        [BsonIgnore]
        public abstract string CollectionKey { get; }
    }
}
