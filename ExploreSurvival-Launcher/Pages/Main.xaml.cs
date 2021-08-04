using ModernWpf.Controls;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ExploreSurvival_Launcher.Pages
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : System.Windows.Controls.Page
    {
        private IniFile config = new IniFile(Environment.CurrentDirectory + "/esl.ini");
        private HttpClient client = new HttpClient();
        public Main()
        {
            InitializeComponent();
            GetNEWS();
        }

        private async void GetNEWS()
        {
            await Task.Run(async () =>
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://www.exploresurvival.ml/news.txt");
                    response.EnsureSuccessStatusCode();
                    StreamReader sr = new StreamReader(await response.Content.ReadAsStreamAsync(), Encoding.UTF8);
                    Dispatcher.Invoke(() =>
                    {
                        while (sr.Peek() >= 0)
                        {
                            //news += sr.ReadLine() + "\n";
                            string message = sr.ReadLine();
                            Match match = Regex.Match(message, "&.");
                            string color = match.Value.Trim('&');
                            Run GetRunWithColor(SolidColorBrush color)
                            {
                                return new Run(message.Remove(0, 2) + "\n")
                                {
                                    Foreground = color
                                };
                            }
                            switch (color)
                            {
                                case "b":
                                    NEWS.Inlines.Add(GetRunWithColor(Brushes.Aqua));
                                    break;
                                case "c":
                                    NEWS.Inlines.Add(GetRunWithColor(Brushes.Red));
                                    break;
                                default:
                                    NEWS.Inlines.Add(message + "\n");
                                    break;
                            }
                        }
                    });
                    sr.Close();
                }
                catch (Exception ex)
                {
                    Dialog("无法加载NEWS", ex.ToString());
                }
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
                // java.exe -Djava.library.path=natives -Xmx512M -jar game.jar <用户名> <sessionID> <uuid>
                if (config.exists("account", "userName") && bool.Parse(config.read("account", "offlineLogin")) && File.Exists("ExploreSurvival/game.jar"))
                {
                    try
                    {
                        string JavaPath = config.read("config", "JavaPath");
                        if (!bool.Parse(config.read("config", "ShowLogs")))
                        {
                            JavaPath = JavaPath.Replace("java.exe", "javaw.exe");
                        }
                        Process p = new Process();
                        p.StartInfo.FileName = JavaPath;
                        p.StartInfo.Arguments = "-Djava.library.path=natives -Xmx" + config.read("config", "JvmMemery") + "M -jar game.jar " + config.read("account", "userName");
                        p.StartInfo.WorkingDirectory = "ExploreSurvival";
                        p.Start();
                        Status.Content = "游戏已启动";
                    }
                    catch (Exception ex)
                    {
                        Dialog("无法启动", ex.ToString());
                    }
                }
                else if (!config.exists("account", "userName"))
                {
                    Dialog("无法启动", "未登录");
                }
                else if (!File.Exists("ExploreSurvival/game.jar"))
                {
                    Dialog("无法启动",  "你没有下载ExploreSurvival");
                }
                else
                {
                    if (CheckSession())
                    {
                        try
                        {
                            string JavaPath = config.read("config", "JavaPath");
                            if (!bool.Parse(config.read("config", "ShowLogs")))
                            {
                                JavaPath = JavaPath.Replace("java.exe", "javaw.exe");
                            }
                            Process p = new Process();
                            p.StartInfo.FileName = JavaPath;
                            p.StartInfo.Arguments = "-Djava.library.path=natives -Xmx" + config.read("config", "JvmMemery") + "M -jar game.jar " + config.read("account", "userName") + " " + config.read("account", "session") + " " + config.read("account", "uuid");
                            p.StartInfo.WorkingDirectory = "ExploreSurvival";
                            p.Start();
                            Status.Content = "游戏已启动";
                        }
                        catch (Exception ex)
                        {
                            Dialog("无法启动", ex.ToString());
                        }
                    }
                    else
                    {
                        config.write("account", "session", "");
                        Dialog("无法启动", "登录过期: 请重新登录");
                    }
                }
            }
            else
            {
                Dialog("注意", "ExploreSurival需要Java,你并没有安装Java\n如果你已经安装了,请前往设置界面配置Java");
            }
        }

        private bool CheckSession()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds() < long.Parse(config.read("account", "expire"));
        }
    }
}
