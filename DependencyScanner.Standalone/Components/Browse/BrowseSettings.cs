using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Standalone.Setting;
using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace DependencyScanner.Standalone.Components.Browse
{
    public class BrowseSettings : ObservableObject, ISettings
    {
        public string Id { get; } = "BrowseSettings";
    }
}
