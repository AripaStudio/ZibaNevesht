using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZibaNevesht.Services;
using ZibaNevesht.ViewModels;

namespace ZibaNevesht.UserControls
{
    /// <summary>
    /// Interaction logic for UCmainZibaNevesht.xaml
    /// </summary>
    public partial class UCmainZibaNevesht : UserControl
    {
        private MainViewModel mainViewModel;
        private bool _isInitialized;
        public UCmainZibaNevesht()
        {
            this.Loaded += OnLoaded;
            InitializeComponent();
            mainViewModel = new MainViewModel();
            DataContext = mainViewModel;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {

            if (_isInitialized)
            {
                return;
            }
            _isInitialized = true;

            try
            {
                await mainViewModel.InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBoxAP.APMessageBox(
                    AlertLevel.Red,
                    "لغزش در آغاز برنامه",
                    $"لغزش در راه اندازی : {ex.Message}",
                    IsASimpleMessageBox: true);
            }
        }
    }
}
