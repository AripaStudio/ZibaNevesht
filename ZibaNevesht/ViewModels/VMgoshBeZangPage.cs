using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZibaNevesht.Services;

namespace ZibaNevesht.ViewModels
{

    public partial class VMgoshBeZangPage : BaseViewModels
    {

      

        [ObservableProperty]
        private string _newItemText;

        [ObservableProperty]
        private ObservableCollection<ZBgoshBeZang> _itemsList;

        [ObservableProperty]
        private ZBgoshBeZang _selectedItem;

        [ObservableProperty]
        private ObservableCollection<AlertLevel> _radehaList;

        [ObservableProperty]
        private AlertLevel _selectedRade;


        IGoshBeZangManager _manager;
        public VMgoshBeZangPage()
        {
            _manager = new GoshBeZangManager();
            RadehaList = new ObservableCollection<AlertLevel>(Enum.GetValues<AlertLevel>());
            ItemsList = new ObservableCollection<ZBgoshBeZang>();
        }
        public static async Task<VMgoshBeZangPage> CreateAsync()
        {
            var vm = new VMgoshBeZangPage();
            await vm.LoadData();
            return vm;
        }

        async Task LoadData()
        {
            try
            {
                var data = await _manager.GetAllData();
                ItemsList = new ObservableCollection<ZBgoshBeZang>(data);
            }
            catch (Exception ex)
            {
                MessageBoxAP.APMessageBox(AlertLevel.Red, "لغزش", $"بارگذاری ناکام ماند: {ex.Message}" , IsASimpleMessageBox:true );
            }
        }



        [RelayCommand]
        private async Task AddItem()
        {
            if (string.IsNullOrWhiteSpace(NewItemText))
            {
                MessageBoxAP.APMessageBox(AlertLevel.Orange, "هشدار", "نوشته ورودی نمیتواند تهی باشد" , IsASimpleMessageBox:true);
                return;
            }

            //if (SelectedRade == default)
            //{
            //    MessageBoxAP.APMessageBox(AlertLevel.Orange, "هشدار", " یک رده برگزینید", IsASimpleMessageBox: true);
            //    return;
            //}

            await _manager.AddOrUpdate(NewItemText, SelectedRade);
            await LoadData();
            NewItemText = "";
        }

        [RelayCommand]
        private async Task DeleteItem()
        {
            if (SelectedItem == null)
            {
                MessageBoxAP.APMessageBox(AlertLevel.Orange, "هشدار", " یک نمونه را برگزینید");
                return;
            }

            var result = MessageBoxAP.APMessageBox(AlertLevel.Orange, "هشدار", $"آیا از پاکسازی '{SelectedItem.NameWord}' دل آسوده هستید؟");
            if (result == MessageBoxAP.MessageBoxAPResult.Deleted)
            {
                await _manager.Remove(SelectedItem.NameWord);
                await LoadData();
            }
        }

        [RelayCommand]
        private async Task DeleteAllItems()
        {
            var result = MessageBoxAP.APMessageBox(AlertLevel.Orange, "هشدار ", "همه موارد حذف خواهند شد. آیا مطمئن هستید؟");
            if (result == MessageBoxAP.MessageBoxAPResult.Deleted)
            {
                await _manager.removeAll();
                await LoadData();
            }
        }
    }
}
