using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace LANSPYproject
{
    public partial class Setting : UserControl
    {
        private const string ConfigFileName = "settings.json";
        private SettingsData currentSettings = new SettingsData();

        public Setting()
        {
            InitializeComponent();
            LoadSettings();
            ApplySettingsToUI();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(ConfigFileName))
                {
                    var json = File.ReadAllText(ConfigFileName);
                    currentSettings = JsonSerializer.Deserialize<SettingsData>(json) ?? new SettingsData();
                }
                else
                {
                    currentSettings = new SettingsData();
                }
            }
            catch
            {
                currentSettings = new SettingsData();
            }
        }

        private void SaveSettingsToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(currentSettings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigFileName, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu file cấu hình: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplySettingsToUI()
        {
            ToggleStartup.IsChecked = currentSettings.StartWithWindows;
            ToggleNewDevice.IsChecked = currentSettings.NotifyNewDevice;
            ToggleDisconnect.IsChecked = currentSettings.NotifyDisconnect;
            ToggleUnknownMAC.IsChecked = currentSettings.NotifyUnknownMAC;
            DeviceThreshold.Text = currentSettings.DeviceThreshold.ToString();
            ScanInterval.Text = currentSettings.ScanInterval.ToString();
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentSettings.StartWithWindows = ToggleStartup.IsChecked == true;
                currentSettings.NotifyNewDevice = ToggleNewDevice.IsChecked == true;
                currentSettings.NotifyDisconnect = ToggleDisconnect.IsChecked == true;
                currentSettings.NotifyUnknownMAC = ToggleUnknownMAC.IsChecked == true;

                if (!int.TryParse(DeviceThreshold.Text, out int deviceThreshold))
                    deviceThreshold = 50;

                if (!int.TryParse(ScanInterval.Text, out int scanInterval))
                    scanInterval = 300;

                currentSettings.DeviceThreshold = deviceThreshold;
                currentSettings.ScanInterval = scanInterval;

                SaveSettingsToFile();

                MessageBox.Show("Đã lưu cài đặt thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu cài đặt: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetDefaults_Click(object sender, RoutedEventArgs e)
        {
            currentSettings = new SettingsData();
            ApplySettingsToUI();
            SaveSettingsToFile();
            MessageBox.Show("Đã khôi phục cài đặt mặc định!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
