using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExploreSurvival_Launcher.Pages
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Page
    {
        private Config config = new Config();
        public Settings()
        {
            InitializeComponent();
            config.Load();
            JvmMemory.Text = config.configData.JVMmemory.ToString();
            JavaPath.Text = config.configData.JavaPath;
            ShowLogs.IsChecked = config.configData.ShowLogs;
            Git_URL.Text = config.configData.GitURL;
            compileOrdownload.SelectedIndex = (int)config.configData.Cod;
        }

        private void ONUM_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            config.configData.JVMmemory = int.Parse(JvmMemory.Text);
            config.configData.JavaPath = JavaPath.Text;
            config.configData.ShowLogs = (bool)ShowLogs.IsChecked;
            config.configData.GitURL = Git_URL.Text;
            config.configData.Cod = (Cod)compileOrdownload.SelectedIndex;
            config.Save();
        }

        private void SelectJavaexe_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "手动查找Java",
                Filter = "Java可执行文件|java.exe"
            };
            if ((bool)ofd.ShowDialog())
            {
                JavaPath.Text = ofd.FileName;
            }
        }

        private void AutoFindJavaexe_Click(object sender, RoutedEventArgs e)
        {
            string Javaexe = Environment.GetEnvironmentVariable("JAVA_HOME") + @"bin\java.exe";
            if (File.Exists(Javaexe))
            {
                JavaPath.Text = Javaexe;
            }
        }
    }
}
