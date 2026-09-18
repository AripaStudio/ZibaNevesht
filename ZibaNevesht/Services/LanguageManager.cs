using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ZibaNevesht.Services
{
    public static class EnumHelper
    {
        public static ObservableCollection<string> ToStringList<T>()
        {
            var list = new ObservableCollection<string>();
            var type = typeof(T);

            foreach (var value in Enum.GetValues(type))
            {
                var field = type.GetField(value.ToString());
                var attr = (DescriptionAttribute)field
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault();

                list.Add(attr?.Description ?? value.ToString());
            }

            return list;
        }
    }

    internal class LanguageManager
    {
        public event EventHandler? LanguageChanged;
        public enum LanguageEnum
        {
            [Description ("Persian")]
            Persian = 0,
            [Description("Persian-Aripa")]
            Aripa_Persian = 1
        }


        private Uri PersianFontPath = new Uri(@"Assets\Languages\PersianLang.xaml", UriKind.Relative);
        private Uri PersianAripaFontPath = new Uri(@"Assets\Languages\Persian_AripaLang.xaml", UriKind.Relative);

        public static string LanguageJsonFilePath = "LanguageZB.json";

        private class LanguageConfig
        {
            public int CurrentLanguage { get; set; }
        }

        private LanguageConfig _config;

        public LanguageManager()
        {
            LoadData();

            ChangeLanguage(GetCurrentLanguage());
        }

        private void LoadData()
        {
            if (File.Exists(LanguageJsonFilePath))
            {
                string json = File.ReadAllText(LanguageJsonFilePath);
                _config = JsonSerializer.Deserialize<LanguageConfig>(json) ?? new LanguageConfig { CurrentLanguage = 0 };
            }
            else
            {
                _config = new LanguageConfig { CurrentLanguage = 0 };
                SaveData();
            }
        }

        private void SaveData()
        {
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_config, options);
            File.WriteAllText(LanguageJsonFilePath, json);
        }

        public LanguageEnum GetCurrentLanguage()
        {
            return (LanguageEnum)_config.CurrentLanguage;
        }

        public void SetCurrentLanguage(LanguageEnum language)
        {
            if (_config.CurrentLanguage == (int)language)
            {
                return;
            }
            _config.CurrentLanguage = (int)language;
            SaveData();
            ChangeLanguage(language);
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }



        private void ChangeLanguage(LanguageEnum lang)
        {
            Uri targetUri = lang == LanguageEnum.Persian ? PersianFontPath : PersianAripaFontPath;
            var currentDict = GetCurrentLanguageResource();

            if (currentDict != null && currentDict.Source == targetUri)
            {
                return;
            }

            if (currentDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(currentDict);
            }

            var newDict = new ResourceDictionary { Source = targetUri };
            Application.Current.Resources.MergedDictionaries.Add(newDict);

        }

        private ResourceDictionary? GetCurrentLanguageResource()
        {
            foreach (ResourceDictionary res in Application.Current.Resources.MergedDictionaries)
            {

                if (res.Source == PersianFontPath || res.Source == PersianAripaFontPath)
                {
                    return res;
                }
            }

            return null;
        }

    }
}
