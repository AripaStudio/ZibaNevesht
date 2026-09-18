using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ZibaNevesht.Services;
using ZibaNevesht.UserControls;

namespace ZibaNevesht.ViewModels
{
    public partial class MainViewModel : BaseViewModels
    {
        [ObservableProperty]
        private VMzibaNeveshChakameNevisanPage vmZibaNevshtChakameNevisanPage;

        [ObservableProperty]
        private VMgoshBeZangPage vmGoshBeZangPage;

        [ObservableProperty]
        private VMmishmarePage vmMishmarePage;

        [ObservableProperty]
        private VMaboutmePage vmAboutmePage;

        [ObservableProperty] private UserControl _currentView;

        [ObservableProperty] private bool _isZibaNeveshtSelected;

        [ObservableProperty] private bool _isGoshBeZangSelected;

        [ObservableProperty] private bool _isMishmarehSelected;

        [ObservableProperty] private bool _isAboutSelected;

        [ObservableProperty] private bool _isDarkTheme;

        [ObservableProperty] private int _selectedLanguageIndex;

        [ObservableProperty] private ObservableCollection<string> _comboBoxItemsSourceLis;

        // Main windows : 
        [ObservableProperty] private int _mainWinHeight;

        [ObservableProperty] private int _mainWinWidth;

        private LanguageManager languageManager;
        public MainViewModel()
        {
            _isZibaNeveshtSelected = true;
            IsDarkTheme = true;

            MainWinHeight = 700;
            MainWinWidth = 900;

            languageManager = new LanguageManager();
           ComboBoxItemsSourceLis = EnumHelper.ToStringList<LanguageManager.LanguageEnum>();

            

            SelectedLanguageIndex = (int)languageManager.GetCurrentLanguage();

        }

        


        public async Task InitializeAsync()
        {
            VmGoshBeZangPage = await VMgoshBeZangPage.CreateAsync();
            VmZibaNevshtChakameNevisanPage = new VMzibaNeveshChakameNevisanPage();
            VmMishmarePage = new VMmishmarePage();
            VmAboutmePage = new VMaboutmePage();
            CurrentView = CreateUCs.UCzibaNeveshtChakameNevisan;


            CreateUCs.UCzibaNeveshtChakameNevisan.DataContext = this;
            CreateUCs.UCgoshBeZang.DataContext = this;
            CreateUCs.UCmishmare.DataContext = this;
            CreateUCs.UCaboutMe.DataContext = this;

                   
            CurrentView = CreateUCs.UCzibaNeveshtChakameNevisan;

        }

        

        partial void OnSelectedLanguageIndexChanged(int value)
        {
            languageManager.SetCurrentLanguage((LanguageManager.LanguageEnum)value);

            bool isAripa = (value == (int)LanguageManager.LanguageEnum.Aripa_Persian);
            LocalizationProvider.Instance.SetLanguage(isAripa);


            var getCurrentLanguage = languageManager.GetCurrentLanguage();
            if (getCurrentLanguage == LanguageManager.LanguageEnum.Persian)
            {
                MainWinHeight = 700;
                MainWinWidth = 900;
            }
            else if (getCurrentLanguage == LanguageManager.LanguageEnum.Aripa_Persian)
            {
                MainWinHeight = 850;
                MainWinWidth = 1200;
            }
            else
            {
                MainWinHeight = 700;
                MainWinWidth = 900;
            }

        }

      
        

        
        partial void OnIsDarkThemeChanged(bool value)
        {
            if (value)
            {
                ThemeManager.CurrentTheme = ThemeManager.ThemeEnum.Dark;
            }
            else
            {
                ThemeManager.CurrentTheme = ThemeManager.ThemeEnum.Light;
            }
        }

        partial void OnIsAboutSelectedChanged(bool value)
        {
            if (value)
            {
                CurrentView = CreateUCs.UCaboutMe;
            }
        }



        partial void OnIsZibaNeveshtSelectedChanged(bool value)
        {
            if (value)
            {
                CurrentView = CreateUCs.UCzibaNeveshtChakameNevisan;
            }
        }

        partial void OnIsGoshBeZangSelectedChanged(bool value)
        {
            if (value)
            {
                CurrentView = CreateUCs.UCgoshBeZang;
            }
        }

        partial void OnIsMishmarehSelectedChanged(bool value)
        {
            if (value)
            {
                CurrentView = CreateUCs.UCmishmare;
            }
        }
    }



    public static class CreateUCs
    {
        private static readonly Lazy<UCzibaNeveshtChakameNevisan> ziba = new(() => new UCzibaNeveshtChakameNevisan());
        private static readonly Lazy<UCgoshBeZang> gosh = new(() => new UCgoshBeZang());
        private static readonly Lazy<UCmishmare> mish = new(() => new UCmishmare());
        private static readonly Lazy<UCaboutMe> about = new(() => new UCaboutMe());

        public static UCzibaNeveshtChakameNevisan UCzibaNeveshtChakameNevisan => ziba.Value;
        public static UCgoshBeZang UCgoshBeZang => gosh.Value;
        public static UCmishmare UCmishmare => mish.Value;
        public static UCaboutMe UCaboutMe => about.Value;
    }
}


