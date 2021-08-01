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
        private IniFile config = new IniFile(Environment.CurrentDirectory + "/esl.ini");
        public Settings()
        {
            InitializeComponent();
            JvmMemery.Text = config.read("config", "JvmMemery");
            JavaPath.Text = config.read("config", "JavaPath");
            ShowLogs.IsChecked = bool.Parse(config.read("config", "ShowLogs"));
        }

        private void ONUM_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            config.write("config", "JvmMemery", JvmMemery.Text);
            config.write("config", "JavaPath", JavaPath.Text);
            config.write("config", "ShowLogs", ShowLogs.IsChecked.ToString());
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
