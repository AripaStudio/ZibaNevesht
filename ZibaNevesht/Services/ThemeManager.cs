using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Packaging;
using System.Security.Policy;
using System.Windows;

namespace ZibaNevesht.Services
{
    public static class ThemeManager
    {

        private static Uri DarkThemePath = new Uri(@"Assets\theme\DarkTheme.xaml", UriKind.Relative);
        private static Uri LightThemePath = new Uri(@"Assets\theme\LightTheme.xaml", UriKind.Relative);
        public enum ThemeEnum
        {
            Light,
            Dark
        }

        private static ThemeEnum _CurrentThemeEnum;
        public static ThemeEnum CurrentTheme
        {
            get => _CurrentThemeEnum;
            set
            {
                ChangeTheme(value);
                _CurrentThemeEnum = value;
            }
        }


        private static void ChangeTheme(ThemeEnum theme)
        {
            ResourceDictionary resource = new ResourceDictionary();
            switch (theme)
            {
                case ThemeEnum.Dark:
                    {


                        resource.Source = DarkThemePath;
                        UpdateTheme();
                        break;
                    }
                case ThemeEnum.Light:
                    {
                        resource.Source = LightThemePath;
                        UpdateTheme();
                        break;
                    }
            }

            void UpdateTheme()
            {
                ThemeEnum EnumCurrentTheme;
                var getCurrentTheme = GetCurrentTheme(out EnumCurrentTheme);
                if (EnumCurrentTheme == theme)
                {
                    return;
                }

                if (getCurrentTheme != null)
                {
                    
                    Application.Current.Resources.MergedDictionaries.Remove(getCurrentTheme);

                    Application.Current.Resources.MergedDictionaries.Add(resource);
                }
            }
        }

        private static ResourceDictionary? GetCurrentTheme(out ThemeEnum geThemeEnum)
        {


            foreach (ResourceDictionary res in Application.Current.Resources.MergedDictionaries)
            {
                if (res.Source == DarkThemePath)
                {
                    geThemeEnum = ThemeEnum.Dark;
                    return res;
                }
                else if (res.Source == LightThemePath)
                {
                    geThemeEnum = ThemeEnum.Light;
                    return res;
                }

            }

            geThemeEnum = ThemeEnum.Light;
            return new ResourceDictionary();
        }


    }
}
