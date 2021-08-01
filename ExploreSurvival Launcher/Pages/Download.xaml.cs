using Microsoft.Win32;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Download()
        {
            InitializeComponent();
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

        private void UAP_TextChanged(object sender, TextChangedEventArgs e)
        {
            Download_BN.IsEnabled = URL.Text != "" && SavePath.Text != "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "保存文件"
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
                        Dialog("错误", ex.ToString());
                    }
                });
            });
        }
    }
}
