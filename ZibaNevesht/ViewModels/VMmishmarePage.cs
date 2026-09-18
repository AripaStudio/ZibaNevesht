using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ZibaNevesht.Services;

namespace ZibaNevesht.ViewModels
{
    public partial class VMmishmarePage : BaseViewModels
    {
        private IMishmareManager mishmareManager;

        private List<ZBmishmare> saveZBmishmare = new List<ZBmishmare>();


        [ObservableProperty] private string _txtUserSearchInput;

        [ObservableProperty] private ObservableCollection<ZBmishmare> _words = new ObservableCollection<ZBmishmare>();
        public VMmishmarePage()
        {
            mishmareManager = new MishmareManager();
            mishmareManager.DataUpdated += MishmareManagerOnDataUpdated;
            _ = LoadData();
            StartRefreshTimer();    
            
        }

        private async void MishmareManagerOnDataUpdated(object? sender, EventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await LoadData();
            });
        }

        private async Task LoadData()
        {
            await GetAndSaveAllList();
            Refresh();
        }


        private DispatcherTimer _uiTimer;
        [ObservableProperty] private string _refreshTimeText = "30s";

        public void StartRefreshTimer()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _uiTimer = new DispatcherTimer();
                _uiTimer.Interval = TimeSpan.FromSeconds(1);
                _uiTimer.Tick += (s, e) =>
                {
                    var diff = mishmareManager.GetNextFlushTime() - DateTime.Now;
                    int seconds = (int)diff.TotalSeconds;

                    if (seconds < 0) seconds = 0;

                    RefreshTimeText = $"{seconds}s";

                };
                _uiTimer.Start();
            });
        }



        [RelayCommand]
        private async Task btnShowSerachingCommand()
        {

            if (string.IsNullOrWhiteSpace(TxtUserSearchInput))
            {
                MessageBox.Show("دوباره بکوشید! ورودی را با نام درست بنوسید.", "ورودی پوچ یا نادرست است", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            var SaveSearched = await mishmareManager.GetOneWordData(TxtUserSearchInput);
            if (SaveSearched != null)
            {
                MessageBox.Show($"نام واژه : {SaveSearched.Name} || شمارگان بهرهمندی : {SaveSearched.NumberOfUsedWord}",
                    "داده های واژه", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            else
            {
                MessageBox.Show("دوباره بکوشید! ورودی را با نام درست بنوسید.", "ورودی پوچ یا نادرست است", MessageBoxButton.OK, MessageBoxImage.Information);
            }



        }


        [RelayCommand]
        private async Task btnRemoveAllCommand()
        {
            await mishmareManager.RemoveAll();
            Refresh();
        }

        void Refresh()
        {
            if (Words != null) Words.Clear();

            foreach (var word in saveZBmishmare)
            {
                Words.Add(word);
            }

        }

        async Task GetAndSaveAllList()
        {
            saveZBmishmare = await mishmareManager.GetAllWords(MishmareManager.ETypeOfResult.FilterData, 50);
        }





    }
}
