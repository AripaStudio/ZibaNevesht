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
using ZibaNevesht.Services;
using ZibaNevesht.ViewModels;

namespace ZibaNevesht
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IZibaNeveshtManager zibaNeveshtManager;
        
        public MainWindow()
        {
            InitializeComponent();
            zibaNeveshtManager = new ZibaNeveshtManager();
            
        }
        
        protected override void OnClosed(EventArgs e)
        {

            if (zibaNeveshtManager.GetIsStart())
            {
                zibaNeveshtManager.Stop();
                base.OnClosed(e);
            }
        }


      
    }
}