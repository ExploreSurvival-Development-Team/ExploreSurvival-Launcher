using Microsoft.Win32;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Handlers;
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

namespace ExploreSurvival_Launcher.Pages
{
    /// <summary>
    /// Download.xaml 的交互逻辑
    /// </summary>
    public partial class Download : System.Windows.Controls.Page
    {
        /*
        private void UAP_TextChanged(object sender, TextChangedEventArgs e)
        {
            Download_BN.IsEnabled = URL.Text != "" && SavePath.Text != "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "保存文件",
                Filter = "所有文件类型|*.*"
            };
            if ((bool)sfd.ShowDialog())
            {
                SavePath.Text = sfd.FileName;
            }
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () =>
            {
                await Dispatcher.Invoke(async () =>
                {
                    try
                    {
                        ProgressMessageHandler pmh = new ProgressMessageHandler(new HttpClientHandler());
                        FileStream fileStream = new FileStream(SavePath.Text, FileMode.Create);
                        pmh.HttpReceiveProgress += (_, e) =>
                        {
                            Dispatcher.Invoke(() =>
                            {
                                Download_PB.Value = e.ProgressPercentage;
                                Download_StatusP.Content = e.ProgressPercentage + "%";
                                if (e.ProgressPercentage == 100)
                                {
                                    Download_Status.Content = "下载完成";
                                }
                                else
                                {
                                    Download_Status.Content = string.Format("{0} bytes / {1} bytes", e.BytesTransferred, e.TotalBytes);
                                }
                            });
                        };
                        HttpClient client = new HttpClient(pmh);
                        Stream stream = await client.GetStreamAsync(URL.Text);
                        await stream.CopyToAsync(fileStream);
                        stream.Close();
                        fileStream.Close();
                    }
                    catch (Exception ex)
                    {
                        Dialog("无法下载", ex.ToString());
                    }
                });
            });
        }
        */
        private IniFile config = new IniFile(Environment.CurrentDirectory + "/esl.ini");
        public Download()
        {
            InitializeComponent();
            Delete.IsEnabled = Directory.Exists("ExploreSurvival");
            Compile.IsEnabled = !Delete.IsEnabled;
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Status.Content = "";
                Delete.IsEnabled = false;
                Compile.IsEnabled = true;
                Directory.Delete("ExploreSurvival", true);
            }
            catch (Exception ex)
            {
                Dialog("错误", ex.ToString());
            }
        }

        private async void Compile_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () =>
            {
                await Dispatcher.Invoke(async () =>
                {
                    if (int.Parse(config.read("config", "cod")) == 0)
                    {
                        Directory.CreateDirectory(".tmp");
                        FileStream fileStream = new FileStream(".tmp/source.zip", FileMode.Create);
                        try
                        {
                            Compile.IsEnabled = false;
                            Delete.IsEnabled = false;
                            ProgressMessageHandler pmh = new ProgressMessageHandler(new HttpClientHandler());
                            pmh.HttpReceiveProgress += (_, e) =>
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    Status.Content = string.Format("已下载 {0} bytes", e.BytesTransferred);
                                });
                            };
                            HttpClient client = new HttpClient(pmh);
                            Stream stream = await client.GetStreamAsync(config.read("config", "GitURL") + "/archive/refs/heads/main.zip");
                            await stream.CopyToAsync(fileStream);
                            stream.Close();
                            fileStream.Close();
                            Status.Content = "下载完成";
                            ZipFile.ExtractToDirectory(".tmp/source.zip", ".tmp/", true);
                            Process p = new Process();
                            p.StartInfo.WorkingDirectory = ".tmp/ExploreSurvival-Game-main/";
                            p.StartInfo.FileName = "release.bat";
                            p.StartInfo.UseShellExecute = true;
                            p.Start();
                            p.WaitForExit();
                            p.Close();
                            Directory.CreateDirectory("ExploreSurvival");
                            Process p2 = new Process();
                            p2.StartInfo.WorkingDirectory = ".tmp/ExploreSurvival-Game-main/out";
                            p2.StartInfo.FileName = "cmd.exe";
                            p2.StartInfo.RedirectStandardInput = true;
                            p2.Start();
                            p2.StandardInput.WriteLine(@"xcopy /e * ..\..\..\ExploreSurvival\&exit");
                            p2.StandardInput.Flush();
                            p2.WaitForExit();
                            p2.Close();
                            Directory.Delete(".tmp", true);
                            Delete.IsEnabled = true;
                        }
                        catch (Exception ex)
                        {
                            fileStream.Close();
                            Directory.Delete(".tmp", true);
                            Dialog("错误", ex.ToString());
                        }
                    }
                    else
                    {
                        Dialog("不支持", "暂不支持");
                    }
                });
            });
        }
    }
}
