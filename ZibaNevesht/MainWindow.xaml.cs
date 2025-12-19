using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZibaNevesht
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyboardManager keyboardManager;
        public MainWindow()
        {
            InitializeComponent();
            keyboardManager = new KeyboardManager();
            
            
        }

        protected override void OnClosed(EventArgs e)
        {
            keyboardManager.Stop();
            base.OnClosed(e);
        }


        private void StartZibaNevesht_OnChecked(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckBoxStartZibaNevesht.IsChecked)
            {
                keyboardManager.Start();
            }
            else
            {
                keyboardManager.Stop();
            }
        }
    }
}