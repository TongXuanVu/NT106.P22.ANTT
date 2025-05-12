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
using System.Windows.Shapes;

namespace LANSPYproject
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Lanspy : Window
    {
        public Lanspy()
        {

        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Dashboard());
        }
        private void ScannerButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Scanner());
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Logs());
        }

        private void AlertsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Alerts());
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Setting());
        }
    }
}
