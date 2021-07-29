using ExploreSurvival_Launcher.Pages;
using ModernWpf.Controls;
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
            if (!config.exists("config", "jvmMemery"))
            {
                config.write("config", "jvmMemery", "1024");
            }
            if (config.exists("account", "userName"))
            {
                User.Content = config.read("account", "userName");
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

                    case "account":
                        frame.Navigate(new Account());
                        break;
                }
            }

        }
    }
}
