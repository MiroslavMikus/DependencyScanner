﻿using MahApps.Metro;
using System.Windows;

namespace DependencyScanner.Standalone.ViewModel
{
    public class AppThemeMenuData : AccentColorMenuData
    {
        protected override void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);

            Properties.Settings.Default.Theme_Name = Name;
            Properties.Settings.Default.Save();
        }
    }
}
