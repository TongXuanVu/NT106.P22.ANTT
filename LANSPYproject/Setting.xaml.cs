using System;
using System.Windows;
using System.Windows.Controls;

namespace LANSPYproject
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : UserControl
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy giá trị từ ToggleButtons
                bool startWithWindows = ToggleStartup.IsChecked == true;
                bool notifyNewDevice = ToggleNewDevice.IsChecked == true;
                bool notifyDisconnect = ToggleDisconnect.IsChecked == true;
                bool notifyUnknownMAC = ToggleUnknownMAC.IsChecked == true;

                // Lấy giá trị số từ TextBoxes
                if (!int.TryParse(DeviceThreshold.Text, out int deviceThreshold))
                    deviceThreshold = 50; // fallback

                if (!int.TryParse(ScanInterval.Text, out int scanInterval))
                    scanInterval = 300; // fallback

                // TODO: Thực hiện lưu các giá trị vào file config hoặc settings
                MessageBox.Show(
                    $"Đã lưu cài đặt:\n" +
                    $"- Khởi động cùng Windows: {startWithWindows}\n" +
                    $"- Cảnh báo thiết bị mới: {notifyNewDevice}\n" +
                    $"- Thiết bị mất kết nối: {notifyDisconnect}\n" +
                    $"- Thiết bị lạ: {notifyUnknownMAC}\n" +
                    $"- Ngưỡng thiết bị: {deviceThreshold}\n" +
                    $"- Thời gian quét lại: {scanInterval} giây",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu cài đặt: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetDefaults_Click(object sender, RoutedEventArgs e)
        {
            // Reset về mặc định
            ToggleStartup.IsChecked = true;
            ToggleNewDevice.IsChecked = false;
            ToggleDisconnect.IsChecked = true;
            ToggleUnknownMAC.IsChecked = true;

            DeviceThreshold.Text = "50";
            ScanInterval.Text = "300";

            MessageBox.Show("Đã khôi phục cài đặt mặc định!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
