using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using static System.Net.Mime.MediaTypeNames;
using Label = System.Windows.Controls.Label;

namespace ZibaNevesht.Services
{
   
    public static class MessageBoxAP
    {
        public enum MessageBoxAPResult
        {
            NotDeleted,
            Deleted
        }
        public static MessageBoxAPResult APMessageBox(AlertLevel level, string saramad, string payam, bool IsASimpleMessageBox = false)
        {
            MessageBoxAPResult result = MessageBoxAPResult.NotDeleted;
            var panjere = new Window
            {
                Height = 280,
                Width = 450,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = Brushes.Transparent
            };

            var charchob = new Border
            {
                CornerRadius = new CornerRadius(12),
                BorderThickness = new Thickness(2),
                Effect = new DropShadowEffect
                {
                    BlurRadius = 15,
                    ShadowDepth = 3,
                    Opacity = 0.5
                }
            };

            var sathBala = new Border
            {
                Height = 50,
                CornerRadius = new CornerRadius(10, 10, 0, 0)
            };

            var namad = new TextBlock
            {
                FontSize = 24,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(15, 0, 0, 0)
            };

            var matnSaramad = new TextBlock
            {
                Text = saramad,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };

            var stackSaramad = new StackPanel
            {
                Orientation =  System.Windows.Controls.Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };
            stackSaramad.Children.Add(namad);
            stackSaramad.Children.Add(matnSaramad);
            sathBala.Child = stackSaramad;

            var matnPayam = new TextBlock
            {
                Text = payam,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(20),
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Right
            };

            var dokme = new System.Windows.Controls.Button()
            {
                Content = "باشه",
                Width = 90,
                Height = 35,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 20, 0)
            };

            var pavaraghi = new TextBlock
            {
                Text = "برای بستن می‌توانید کلید Enter را بفشارید",
                FontSize = 10,
                FontStyle = FontStyles.Italic,
                Opacity = 0.7,
                Margin = new Thickness(15, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            var sathPaein = new Border
            {
                Height = 60,
                CornerRadius = new CornerRadius(0, 0, 10, 10)
            };

            var stackPaein = new DockPanel();
            DockPanel.SetDock(pavaraghi, Dock.Left);
            stackPaein.Children.Add(pavaraghi);
            stackPaein.Children.Add(dokme);
            sathPaein.Child = stackPaein;

            var stackAsli = new DockPanel();
            DockPanel.SetDock(sathBala, Dock.Top);
            DockPanel.SetDock(sathPaein, Dock.Bottom);
            stackAsli.Children.Add(sathBala);
            stackAsli.Children.Add(sathPaein);
            stackAsli.Children.Add(matnPayam);

            charchob.Child = stackAsli;
            panjere.Content = charchob;

            switch (level)
            {
                case AlertLevel.White:
                    charchob.BorderBrush = new SolidColorBrush(Color.FromRgb(180, 180, 180));
                    charchob.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                    sathBala.Background = new SolidColorBrush(Color.FromRgb(100, 120, 100));
                    sathPaein.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    namad.Text = "ℹ";
                    namad.Foreground = Brushes.White;
                    matnSaramad.Foreground = Brushes.White;
                    matnPayam.Foreground = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                    pavaraghi.Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80));
                    dokme.Background = new SolidColorBrush(Color.FromRgb(100, 120, 100));
                    dokme.Foreground = Brushes.White;
                    break;

                case AlertLevel.Orange:
                    charchob.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 140, 0));
                    charchob.Background = new SolidColorBrush(Color.FromRgb(255, 248, 220));
                    sathBala.Background = new SolidColorBrush(Color.FromRgb(255, 140, 0));
                    sathPaein.Background = new SolidColorBrush(Color.FromRgb(255, 235, 200));
                    namad.Text = "⚠";
                    namad.Foreground = Brushes.White;
                    matnSaramad.Foreground = Brushes.White;
                    matnPayam.Foreground = new SolidColorBrush(Color.FromRgb(60, 60, 60));
                    pavaraghi.Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                    dokme.Background = new SolidColorBrush(Color.FromRgb(255, 140, 0));
                    dokme.Foreground = Brushes.White;
                    break;

                case AlertLevel.Red:
                    charchob.BorderBrush = new SolidColorBrush(Color.FromRgb(180, 0, 0));
                    charchob.Background = new SolidColorBrush(Color.FromRgb(255, 235, 235));
                    sathBala.Background = new SolidColorBrush(Color.FromRgb(180, 0, 0));
                    sathPaein.Background = new SolidColorBrush(Color.FromRgb(255, 220, 220));
                    namad.Text = "⛔";
                    namad.Foreground = Brushes.White;
                    matnSaramad.Foreground = Brushes.White;
                    matnPayam.Foreground = new SolidColorBrush(Color.FromRgb(60, 0, 0));
                    pavaraghi.Foreground = new SolidColorBrush(Color.FromRgb(100, 0, 0));
                    dokme.Background = new SolidColorBrush(Color.FromRgb(180, 0, 0));
                    dokme.Foreground = Brushes.White;
                    break;
            }

            void bandKon(object sender, RoutedEventArgs e)
            {
                
                if (level == AlertLevel.Red)
                {
                    result = MessageBoxAPResult.Deleted;
                }
                else if (level == AlertLevel.Orange && !IsASimpleMessageBox)
                {
                    var yadavari = CreateYadavari(AlertLevel.Orange);
                    yadavari.ShowDialog();
                    
                }

                panjere.Close();


                Window CreateYadavari(AlertLevel level)
                {
                    var yadavari = new Window
                    {
                        Height = 150,
                        Width = 320,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ResizeMode = ResizeMode.NoResize,
                        Title = "یادآوری"
                    };
                    System.Windows.Controls.Button dokmeDeletedInput = new System.Windows.Controls.Button
                    {
                        Width = 130,
                        Height = 30,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        Margin = new Thickness(0, 10, 0, 15),
                        VerticalAlignment = VerticalAlignment.Bottom
                    };
                    System.Windows.Controls.Button dokmeYadavari = new System.Windows.Controls.Button
                    {
                        Width = 130,
                        Height = 30,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        Margin = new Thickness(0, 10, 0, 15),
                        VerticalAlignment = VerticalAlignment.Bottom
                    };

                    var textYadavari = new TextBlock
                    {                        
                        FontSize = 13,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(15),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    switch (level)
                    {
                        case AlertLevel.Red:
                            textYadavari.Text = "نوشته شما به چرایی آنکه در رده سرخ است جایگذاری نمیشود!";
                            dokmeYadavari.Content = "دریافتم";
                            break;
                        case AlertLevel.Orange:

                            textYadavari.Text = "نوشته شما در رده نیمه سرخ(نارنگی) است/ ایا میخواهید واژه نوشته شده  پالایش شود یا نه؟";
                            dokmeDeletedInput.Content = "پالایش شود";
                            dokmeYadavari.Content = "پالایش نشود";
                            break;
                        default:
                            return new Window();                            
                    }


                    dokmeYadavari.Click += (s, args) =>
                    {
                        result = MessageBoxAPResult.NotDeleted;
                        yadavari.Close();
                    };

                    dokmeDeletedInput.Click += (s, args) =>
                    {
                        result = MessageBoxAPResult.Deleted;
                        yadavari.Close();
                    };
                    yadavari.KeyDown += (s, args) =>
                    {
                        if (args.Key == Key.Enter)
                        {
                            if (level == AlertLevel.Orange)
                                result = MessageBoxAPResult.Deleted;
                            yadavari.Close();
                        }
                    };

                    var stackYadavari = new StackPanel();
                    stackYadavari.Children.Add(textYadavari);

                    
                    var stackDokme = new StackPanel
                    {
                        Orientation = System.Windows.Controls.Orientation.Horizontal,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        Margin = new Thickness(0, 10, 0, 0)
                    };

                    stackDokme.Children.Add(dokmeYadavari);
                    if (level == AlertLevel.Orange)
                    {
                        stackDokme.Children.Add(new System.Windows.Controls.Border { Width = 10 });
                        stackDokme.Children.Add(dokmeDeletedInput);
                    }

                    stackYadavari.Children.Add(stackDokme);
                    yadavari.Content = stackYadavari;
                    yadavari.Background = new SolidColorBrush(Color.FromRgb(255, 240, 240));

                    return yadavari;
                }
            }

             dokme.Click += bandKon;
            panjere.KeyDown += (s, e) => { if (e.Key == Key.Enter) bandKon(null, null); };
            
            panjere.ShowDialog();
            return result;
            
        }
    }
}
