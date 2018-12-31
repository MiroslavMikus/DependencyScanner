using GalaSoft.MvvmLight;
using LiteDB;

namespace DependencyScanner.Standalone.Setting
{
    public abstract class ObservableSettingsBase : ObservableObject, ISettings
    {
        [BsonId]
        public string Id { get; } = SettingsManager.DefaultKey;
    }
}
