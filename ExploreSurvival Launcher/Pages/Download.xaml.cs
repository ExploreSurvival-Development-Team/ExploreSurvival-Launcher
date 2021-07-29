using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Text;
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
            new ContentDialog
            {
                Title = "服务器离线",
                Content = "无法连接到ExploreSurvival服务器",
                CloseButtonText = "OK"
            }.ShowAsync();
        }
    }
}
