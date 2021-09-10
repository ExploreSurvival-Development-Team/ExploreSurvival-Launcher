using ExploreSurvival_Launcher.Pages;
using ModernWpf.Controls;
using System;
using System.IO;
using System.Windows;

namespace ExploreSurvival_Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavView.SelectedItem = NavView.MenuItems[0];
            frame.Navigate(new Main());
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                frame.Navigate(new Settings());
            }
            else
            {
                switch (args.InvokedItemContainer.Tag)
                {
                    case "startgame":
                        frame.Navigate(new Main());
                        break;

                    case "download":
                        frame.Navigate(new Download());
                        break;

                    case "account":
                        frame.Navigate(new Account());
                        break;
                }
            }

        }
    }
}
