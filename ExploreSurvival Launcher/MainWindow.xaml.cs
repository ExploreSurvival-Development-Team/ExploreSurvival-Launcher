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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            StartLoadline.Visibility = Visibility;
        }
        private void SwitchLang_enUS(object sender, RoutedEventArgs e)
        {
            Title = "ExploreSurvival Launcher";
            LoginUsernameText.Text = "Username";
            LoginPasswordText.Text = "Password";
            StartGame.Content = "Start Game";
            versionsText.Text = "Versions";
            SettingText.Text = "Options";
            RunmemoryText.Text = "JVM Memory";
            LangText.Text = "Language";
            OfflineLogin.Content = "Offline Login";
            ChangePasswordButton.Content = "Change the password";
            AccountOperationButton.Content = "Account operation";
        }

        private void SwitchLang_zhCN(object sender, RoutedEventArgs e)
        {
            Title = "ExploreSurvival 启动器";
            LoginUsernameText.Text = "用户名";
            LoginPasswordText.Text = "密码";
            StartGame.Content = "启动游戏";
            versionsText.Text = "版本";
            SettingText.Text = "设置";
            RunmemoryText.Text = "运行内存(MB)";
            LangText.Text = "语言";
            OfflineLogin.Content = "离线登录";
            ChangePasswordButton.Content = "修改密码";
            AccountOperationButton.Content = "账户操作";
        }

        private void RunMSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9)              //大键盘0-9
                || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)   //小键盘0-9 
                || e.Key == Key.Delete || e.Key == Key.Back         //Delete Backspace
                || e.Key == Key.Right || e.Key == Key.Left))        //左右方向
            {
                e.Handled = true;
            }
        }
    }
}
