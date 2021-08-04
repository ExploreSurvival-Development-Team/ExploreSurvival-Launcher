using ModernWpf.Controls;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExploreSurvival_Launcher.Pages
{
    /// <summary>
    /// Account.xaml 的交互逻辑
    /// </summary>
    public partial class Account : System.Windows.Controls.Page
    {
        private IniFile config = new IniFile(Environment.CurrentDirectory + "/esl.ini");
        private HttpClient client = new HttpClient();
        public Account()
        {
            InitializeComponent();
            if (!config.exists("account", "session") && !bool.Parse(config.read("account", "offlineLogin")))
            {
                HideLoginAfter();
                ShowLogin();
                userName.Text = config.read("account", "userName");
            }
            else if (config.exists("account", "userName"))
            {
                HideLogin();
                Welcome.Content += config.read("account", "userName");
            }
            else
            {
                HideLoginAfter();
            }
        }

        private void Dialog(string Title, string Content)
        {
            new ContentDialog
            {
                Title = Title,
                Content = Content,
                CloseButtonText = "OK"
            }.ShowAsync();
        }
        private void HideLogin()
        {
            userName.Visibility = Visibility.Hidden;
            userNameL.Visibility = Visibility.Hidden;
            userPass.Visibility = Visibility.Hidden;
            userPassL.Visibility = Visibility.Hidden;
            OfflineLogin.Visibility = Visibility.Hidden;
            login.Visibility = Visibility.Hidden;
        }

        private void ShowLogin()
        {
            userName.Visibility = Visibility.Visible;
            userNameL.Visibility = Visibility.Visible;
            userPass.Visibility = Visibility.Visible;
            userPassL.Visibility = Visibility.Visible;
            OfflineLogin.Visibility = Visibility.Visible;
            login.Visibility = Visibility.Visible;
        }

        private void HideLoginAfter()
        {
            Logout.Visibility = Visibility.Hidden;
            Welcome.Visibility = Visibility.Hidden;
        }
        private void ShowLoginAfter()
        {
            Logout.Visibility = Visibility.Visible;
            Welcome.Visibility = Visibility.Visible;
        }


        private async void login_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)OfflineLogin.IsChecked)
            {
                config.write("account", "userName", userName.Text);
                config.write("account", "offlineLogin", "True");
                HideLogin();
                Restart("登录成功");
            }
            else
            {
                try
                {
                    login.IsEnabled = false;
                    userName.IsEnabled = false;
                    userPass.IsEnabled = false;
                    OfflineLogin.IsEnabled = false;
                    HttpResponseMessage response = await client.GetAsync("https://www.opencomputers.ml:7331/ExploreSurvival/login.jsp?username=" + userName.Text + "&password=" + userPass.Password);
                    response.EnsureSuccessStatusCode();
                    Response data = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
                    if (data.success)
                    {
                        config.write("account", "userName", userName.Text);
                        config.write("account", "offlineLogin", "False");
                        config.write("account", "session", data.session);
                        config.write("account", "uuid", data.uuid);
                        config.write("account", "expire", data.expire.ToString());
                        Restart("登录成功");
                    }
                    else
                    {
                        Dialog("无法登录", "用户名或密码错误");
                        login.IsEnabled = true;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Dialog("无法登录", ex.ToString());
                }
            }
        }

        private async void Restart(string Title)
        {
            await new ContentDialog
            {
                Title = Title,
                Content = "启动器需要重新启动",
                CloseButtonText = "OK"
            }.ShowAsync();
            System.Diagnostics.Process.Start(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Application.Current.Shutdown();
        }

        private void OfflineLogin_Checked(object sender, RoutedEventArgs e)
        {
            userPass.IsEnabled = false;
            login.IsEnabled = userName.Text != "";
        }

        private void OfflineLogin_Unchecked(object sender, RoutedEventArgs e)
        {
            userPass.IsEnabled = true;
            login.IsEnabled = userPass.Password != "";
        }

        private void userName_TextChanged(object sender, TextChangedEventArgs e)
        {
            login.IsEnabled = (userName.Text != "" && userPass.Password != "") || ((bool)OfflineLogin.IsChecked && userName.Text != "");
        }

        private void userPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            login.IsEnabled = userName.Text != "" && userPass.Password != "";
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            config.write("account", "userName", "");
            HideLoginAfter();
            ShowLogin();
            Restart("注销成功");
        }

        private void UAP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ((userName.Text != "" && userPass.Password != "") || ((bool)OfflineLogin.IsChecked && userName.Text != "")))
            {
                login_Click(null, null);
            }
        }
    }
    public class Response
    {
        public bool success;
        public string reason;
        public string username;
        public string session;
        public string uuid;
        public long expire;
    }
}
