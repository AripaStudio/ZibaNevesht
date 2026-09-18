using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using ZibaNevesht.Assets.Resource;

namespace ZibaNevesht.Services
{
    public sealed class LocalizationProvider : INotifyPropertyChanged
    {
        private static LocalizationProvider _instance;
        public static LocalizationProvider Instance => _instance ??= new LocalizationProvider();

        private readonly ResourceManager _persianManager;
        private readonly ResourceManager _aripaManager;
        private ResourceManager _currentManager;

        private LocalizationProvider()
        {
            _persianManager = Assets.Resource.LanguageResources.ResourceManager;
            _aripaManager = Assets.Resource.LanguageResources_peap.ResourceManager;
            _currentManager = _persianManager;

            Debug.WriteLine($"[LocalizationProvider] Persian Manager: {_persianManager != null}");
            Debug.WriteLine($"[LocalizationProvider] Aripa Manager: {_aripaManager != null}");
        }

        public string this[string key]
        {
            get
            {
                var result = _currentManager.GetString(key);
                Debug.WriteLine($"[LocalizationProvider] Key: {key}, Value: {result}, IsAripa: {_currentManager == _aripaManager}");
                return result ?? key;
            }
        }

        public void SetLanguage(bool isAripa)
        {
            _currentManager = isAripa ? _aripaManager : _persianManager;
            Debug.WriteLine($"[LocalizationProvider] Language changed to: {(isAripa ? "Aripa" : "Persian")}");

            OnPropertyChanged("Item[]");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null)
                return;

            var dispatcher = Application.Current?.Dispatcher;
            if (dispatcher != null && !dispatcher.CheckAccess())
            {
                dispatcher.Invoke(() => handler(this, new PropertyChangedEventArgs(propertyName)));
            }
            else
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
