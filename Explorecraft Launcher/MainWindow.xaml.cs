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

namespace Explorecraft_Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            this.StartLoadline.Visibility = Visibility.Visible;
        }

        private void SwitchLang_enUS(object sender, RoutedEventArgs e)
        {
            this.Title = "ExploreSurvival Launcher";
            this.LoginUsernameText.Text = "Username";
            this.LoginPasswordText.Text = "Password";
            this.StartGame.Content = "Start Game";
            this.versionsText.Text = "Versions";
            this.SettingText.Text = "Options";
            this.RunmemoryText.Text = "JVM Memory";
            this.LangText.Text = "Language";
            this.LixianLogin.Content = "Offline Login";
            this.ChangePasswordButton.Content = "Change the password";
            this.AccountOperationButton.Content = "Account operation";
        }

        private void SwitchLang_zhCN(object sender, RoutedEventArgs e)
        {
            this.Title = "ExploreSurvival 启动器";
            this.LoginUsernameText.Text = "用户名";
            this.LoginPasswordText.Text = "密码";
            this.StartGame.Content = "启动游戏";
            this.versionsText.Text = "版本";
            this.SettingText.Text = "设置";
            this.RunmemoryText.Text = "运行内存";
            this.LangText.Text = "语言";
            this.LixianLogin.Content = "离线登录";
            this.ChangePasswordButton.Content = "修改密码";
            this.AccountOperationButton.Content = "账户操作";
        }
    }
}
