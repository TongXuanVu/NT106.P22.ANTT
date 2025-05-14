using System.Windows;
using System.Windows.Controls;

namespace LANSPYproject
{
    public partial class Lanspy : Window
    {
        public Lanspy()
        {
            InitializeComponent();
            // Mặc định mở trang Dashboard khi khởi động
            MainContent.Content = new Dashboard();
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Dashboard();
        }

        private void ScannerButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Scanner();
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Logs();
        }

        private void AlertsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Alerts();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Setting();
        }
    }
}
