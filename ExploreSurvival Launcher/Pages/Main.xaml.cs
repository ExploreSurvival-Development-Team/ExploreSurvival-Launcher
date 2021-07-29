using ModernWpf.Controls;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExploreSurvival_Launcher.Pages
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : System.Windows.Controls.Page
    {
        private IniFile config = new IniFile(Environment.CurrentDirectory + "/esl.ini");
        public Main()
        {
            InitializeComponent();
            Autorun();
        }
        private async void Autorun()
        {
            NEWS.Text = await GetNEWS();
        }

        private async Task<string> GetNEWS()
        {
            return await Task.Run(() =>
            {
                HttpWebRequest request = WebRequest.CreateHttp("http://www.exploresurvival.ml/news.txt");
                request.Method = "GET";
                request.Timeout = 5000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string news = sr.ReadToEnd();
                sr.Close();
                response.Close();
                return news;
            });
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

        private bool CheckJava()
        {
            if (config.exists("config", "JavaPath"))
            {
                if (!File.Exists(config.read("config", "JavaPath")))
                {
                    return false;
                }
            }
            return true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckJava())
            {
                Dialog("无法启动", "LAZY");
            }
            else
            {
                Dialog("注意", "ExploreSurival需要Java,你并没有安装Java\n如果你已经安装了,请前往设置界面配置Java");
            }
        }
    }
}
