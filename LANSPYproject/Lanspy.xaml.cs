using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace LANSPYproject
{
    public partial class Lanspy : Window
    {
        private DispatcherTimer wifiTimer;

        // Khởi tạo và lưu các instance trang
        private Dashboard dashboardPage = new Dashboard();
        private Scanner scannerPage = new Scanner();
        private Logs logsPage = new Logs();
        private Alerts alertsPage = new Alerts();
        private Setting settingPage = new Setting();

        public Lanspy()
        {
            InitializeComponent();

            UpdateWifiDisplay();

            MainContent.Content = dashboardPage;

            // Cập nhật tên Wifi mỗi 10 giây
            wifiTimer = new DispatcherTimer();
            wifiTimer.Interval = TimeSpan.FromSeconds(10);
            wifiTimer.Tick += (s, e) => UpdateWifiDisplay();
            wifiTimer.Start();
        }

        private void UpdateWifiDisplay()
        {
            string wifiName = GetWifiName();

            if (string.IsNullOrEmpty(wifiName) || wifiName == "Unknown")
            {
                WifiNameTextBlock.Text = "Không có kết nối Wifi";
                WifiNameTextBlock.Foreground = Brushes.Red;
                WifiNameTextBlock.ToolTip = "Hiện tại máy không kết nối Wifi";
            }
            else
            {
                WifiNameTextBlock.Text = wifiName;
                WifiNameTextBlock.Foreground = Brushes.Black;
                WifiNameTextBlock.ToolTip = $"Đang kết nối Wifi: {wifiName}";
            }
        }

        private string GetWifiName()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("netsh", "wlan show interfaces")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    Regex regex = new Regex(@"^\s*SSID\s*:\s*(.+)$", RegexOptions.Multiline);
                    Match match = regex.Match(output);
                    if (match.Success)
                    {
                        string ssid = match.Groups[1].Value.Trim();
                        if (ssid.Equals("BSSID", StringComparison.OrdinalIgnoreCase))
                            return "Unknown";
                        return ssid;
                    }
                }
            }
            catch
            {
                // Lỗi thì trả về Unknown
            }
            return "Unknown";
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = dashboardPage;
        }

        private void ScannerButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = scannerPage;
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = logsPage;
        }

        private void AlertsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = alertsPage;
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = settingPage;
        }
    }
}
