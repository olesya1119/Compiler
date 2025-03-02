using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Compiler.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpenGitHubLink(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string githubUrl = "https://github.com/olesya1119/Compiler"; // Укажи ссылку на свой репозиторий
            Process.Start(new ProcessStartInfo(githubUrl) { UseShellExecute = true });
        }
    }
}
