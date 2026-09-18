using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZibaNevesht.Services;

namespace ZibaNevesht.ViewModels
{
    public partial class VMaboutmePage : BaseViewModels
    {

        [RelayCommand]
        private void OpenWebsiteCommand()
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = "https://aripapars.top/",
                UseShellExecute = true,
            });
        }

        [RelayCommand]
        private void OpenTelegramChannelCommand()
        {

            Process.Start(new ProcessStartInfo()
            {
                FileName = "https://t.me/AripaStudio",
                UseShellExecute = true,
            });
        }

        [RelayCommand]
        private void OpenGithubCommand()
        {

            Process.Start(new ProcessStartInfo()
            {
                FileName = "https://github.com/AripaStudio",
                UseShellExecute = true,
            });
        }

        [RelayCommand]
        private void OpenPersonalTelegramCommand()
        {

            Process.Start(new ProcessStartInfo()
            {
                FileName = "https://t.me/khashayarAP",
                UseShellExecute = true,
            });
        }

    }
}
