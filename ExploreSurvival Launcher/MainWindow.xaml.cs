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
        private IniFile config = new IniFile(Environment.CurrentDirectory + "/esl.ini");
        public MainWindow()
        {
            InitializeComponent();
            if (!config.exists("config", "JvmMemery"))
            {
                config.write("config", "JvmMemery", "1024");
            }
            if (!config.exists("config", "ShowLogs"))
            {
                config.write("config", "ShowLogs", "False");
            }
            if (!config.exists("config", "GitURL"))
            {
                config.write("config", "GitURL", "https://github.com/493505110/ExploreSurvival-Game");
            }
            if (config.exists("account", "userName"))
            {
                User.Content = config.read("account", "userName");
            }
            string Javaexe = Environment.GetEnvironmentVariable("JAVA_HOME") + @"bin\java.exe";
            if (File.Exists(Javaexe))
            {
                config.write("config", "JavaPath", Javaexe);
            }
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
