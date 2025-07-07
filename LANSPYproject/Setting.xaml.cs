using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;

namespace LANSPYproject
{
    /// <summary>
    /// UserControl cho trang cài đặt ứng dụng
    /// </summary>
    public partial class Setting : UserControl
    {
        private const string ConfigFileName = "settings.json";
        private const string AppName = "LANSPYproject";
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        private SettingsData currentSettings = new SettingsData();

        public Setting()
        {
            InitializeComponent();
            LoadSettings();
            ApplySettingsToUI();

            // Gán sự kiện cho các control
            AttachEventHandlers();
        }

        #region Event Handlers Registration
        /// <summary>
        /// Gán các sự kiện cho các control
        /// </summary>
        private void AttachEventHandlers()
        {
            // Sự kiện khi toggle thay đổi
            ToggleStartup.Checked += OnToggleStartup_Changed;
            ToggleStartup.Unchecked += OnToggleStartup_Changed;

            // Sự kiện khi TextBox mất focus để validate
            DeviceThreshold.LostFocus += ValidateDeviceThreshold;
            ScanInterval.LostFocus += ValidateScanInterval;

            // Sự kiện chỉ cho phép nhập số
            DeviceThreshold.PreviewTextInput += NumericOnly_PreviewTextInput;
            ScanInterval.PreviewTextInput += NumericOnly_PreviewTextInput;
        }
        #endregion

        #region Settings Load/Save
        /// <summary>
        /// Tải cài đặt từ file JSON
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                if (File.Exists(ConfigFileName))
                {
                    var json = File.ReadAllText(ConfigFileName);
                    var loadedSettings = JsonSerializer.Deserialize<SettingsData>(json);

                    if (loadedSettings != null && loadedSettings.IsValid())
                    {
                        currentSettings = loadedSettings;
                    }
                    else
                    {
                        currentSettings = new SettingsData();
                        ShowMessage("Cài đặt không hợp lệ, đã khôi phục mặc định.", "Cảnh báo", MessageBoxImage.Warning);
                    }
                }
                else
                {
                    currentSettings = new SettingsData();
                    SaveSettingsToFile(); // Tạo file cài đặt mặc định
                }
            }
            catch (Exception ex)
            {
                currentSettings = new SettingsData();
                ShowMessage($"Lỗi khi tải cài đặt: {ex.Message}", "Lỗi", MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Lưu cài đặt vào file JSON
        /// </summary>
        private void SaveSettingsToFile()
        {
            try
            {
                // Chỉ lưu các thuộc tính cần thiết, bỏ qua CreatedAt và LastUpdated
                var settingsToSave = new
                {
                    StartWithWindows = currentSettings.StartWithWindows,
                    NotifyNewDevice = currentSettings.NotifyNewDevice,
                    NotifyDisconnect = currentSettings.NotifyDisconnect,
                    NotifyUnknownMAC = currentSettings.NotifyUnknownMAC,
                    DeviceThreshold = currentSettings.DeviceThreshold,
                    ScanInterval = currentSettings.ScanInterval
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var json = JsonSerializer.Serialize(settingsToSave, options);
                File.WriteAllText(ConfigFileName, json);
            }
            catch (Exception ex)
            {
                ShowMessage($"Lỗi lưu file cấu hình: {ex.Message}", "Lỗi", MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Áp dụng cài đặt lên giao diện
        /// </summary>
        private void ApplySettingsToUI()
        {
            ToggleStartup.IsChecked = currentSettings.StartWithWindows;
            ToggleNewDevice.IsChecked = currentSettings.NotifyNewDevice;
            ToggleDisconnect.IsChecked = currentSettings.NotifyDisconnect;
            ToggleUnknownMAC.IsChecked = currentSettings.NotifyUnknownMAC;
            DeviceThreshold.Text = currentSettings.DeviceThreshold.ToString();
            ScanInterval.Text = currentSettings.ScanInterval.ToString();
        }

        /// <summary>
        /// Thu thập cài đặt từ giao diện
        /// </summary>
        private void CollectSettingsFromUI()
        {
            currentSettings.StartWithWindows = ToggleStartup.IsChecked == true;
            currentSettings.NotifyNewDevice = ToggleNewDevice.IsChecked == true;
            currentSettings.NotifyDisconnect = ToggleDisconnect.IsChecked == true;
            currentSettings.NotifyUnknownMAC = ToggleUnknownMAC.IsChecked == true;

            // Xử lý số nguyên với validation
            if (!int.TryParse(DeviceThreshold.Text, out int deviceThreshold) || deviceThreshold <= 0)
            {
                deviceThreshold = 50;
                DeviceThreshold.Text = "50";
            }

            if (!int.TryParse(ScanInterval.Text, out int scanInterval) || scanInterval < 30)
            {
                scanInterval = 300;
                ScanInterval.Text = "300";
            }

            currentSettings.DeviceThreshold = Math.Min(deviceThreshold, 1000);
            currentSettings.ScanInterval = Math.Max(scanInterval, 30);
        }
        #endregion

        #region Windows Startup Management
        /// <summary>
        /// Xử lý khi toggle khởi động Windows thay đổi
        /// </summary>
        private void OnToggleStartup_Changed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ToggleStartup.IsChecked == true)
                {
                    AddToWindowsStartup();
                }
                else
                {
                    RemoveFromWindowsStartup();
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Lỗi khi thay đổi cài đặt khởi động: {ex.Message}", "Lỗi", MessageBoxImage.Error);
                // Khôi phục trạng thái cũ
                ToggleStartup.IsChecked = !ToggleStartup.IsChecked;
            }
        }

        /// <summary>
        /// Thêm ứng dụng vào danh sách khởi động Windows
        /// </summary>
        private void AddToWindowsStartup()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key != null)
                    {
                        string appPath = Assembly.GetExecutingAssembly().Location;
                        if (appPath.EndsWith(".dll"))
                        {
                            appPath = appPath.Replace(".dll", ".exe");
                        }

                        key.SetValue(AppName, $"\"{appPath}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể thêm vào khởi động Windows: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa ứng dụng khỏi danh sách khởi động Windows
        /// </summary>
        private void RemoveFromWindowsStartup()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key != null)
                    {
                        key.DeleteValue(AppName, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể xóa khỏi khởi động Windows: {ex.Message}");
            }
        }

        /// <summary>
        /// Kiểm tra xem ứng dụng có trong danh sách khởi động Windows không
        /// </summary>
        private bool IsInWindowsStartup()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
                {
                    return key?.GetValue(AppName) != null;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Input Validation
        /// <summary>
        /// Chỉ cho phép nhập số
        /// </summary>
        private void NumericOnly_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        /// <summary>
        /// Validate ngưỡng số lượng thiết bị
        /// </summary>
        private void ValidateDeviceThreshold(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(DeviceThreshold.Text, out int value))
            {
                if (value <= 0)
                {
                    DeviceThreshold.Text = "1";
                    ShowMessage("Ngưỡng thiết bị phải lớn hơn 0", "Cảnh báo", MessageBoxImage.Warning);
                }
                else if (value > 1000)
                {
                    DeviceThreshold.Text = "1000";
                    ShowMessage("Ngưỡng thiết bị không được vượt quá 1000", "Cảnh báo", MessageBoxImage.Warning);
                }
            }
            else
            {
                DeviceThreshold.Text = "50";
                ShowMessage("Vui lòng nhập số hợp lệ", "Cảnh báo", MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Validate thời gian quét
        /// </summary>
        private void ValidateScanInterval(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ScanInterval.Text, out int value))
            {
                if (value < 30)
                {
                    ScanInterval.Text = "30";
                    ShowMessage("Thời gian quét tối thiểu là 30 giây", "Cảnh báo", MessageBoxImage.Warning);
                }
                else if (value > 3600)
                {
                    ScanInterval.Text = "3600";
                    ShowMessage("Thời gian quét tối đa là 3600 giây (1 giờ)", "Cảnh báo", MessageBoxImage.Warning);
                }
            }
            else
            {
                ScanInterval.Text = "300";
                ShowMessage("Vui lòng nhập số hợp lệ", "Cảnh báo", MessageBoxImage.Warning);
            }
        }
        #endregion

        #region Button Events
        /// <summary>
        /// Sự kiện nút Lưu cài đặt
        /// </summary>
        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CollectSettingsFromUI();

                // Validate cài đặt
                if (!currentSettings.IsValid())
                {
                    ShowMessage("Cài đặt không hợp lệ. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxImage.Error);
                    return;
                }

                SaveSettingsToFile();

                ShowMessage("Đã lưu cài đặt thành công!", "Thông báo", MessageBoxImage.Information);

                // Gọi event để thông báo cài đặt đã thay đổi
                OnSettingsChanged?.Invoke(currentSettings);
            }
            catch (Exception ex)
            {
                ShowMessage($"Lỗi khi lưu cài đặt: {ex.Message}", "Lỗi", MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Sự kiện nút Khôi phục mặc định
        /// </summary>
        private void ResetDefaults_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn khôi phục tất cả cài đặt về mặc định?",
                    "Xác nhận",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    currentSettings.ResetToDefaults();
                    ApplySettingsToUI();
                    SaveSettingsToFile();

                    ShowMessage("Đã khôi phục cài đặt mặc định!", "Thông báo", MessageBoxImage.Information);

                    // Gọi event để thông báo cài đặt đã thay đổi
                    OnSettingsChanged?.Invoke(currentSettings);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Lỗi khi khôi phục cài đặt: {ex.Message}", "Lỗi", MessageBoxImage.Error);
            }
        }
        #endregion

        #region Public Properties and Events
        /// <summary>
        /// Event được kích hoạt khi cài đặt thay đổi
        /// </summary>
        public event Action<SettingsData> OnSettingsChanged;

        /// <summary>
        /// Lấy cài đặt hiện tại
        /// </summary>
        public SettingsData CurrentSettings => currentSettings;

        /// <summary>
        /// Cập nhật cài đặt từ bên ngoài
        /// </summary>
        public void UpdateSettings(SettingsData newSettings)
        {
            if (newSettings != null && newSettings.IsValid())
            {
                currentSettings = newSettings;
                ApplySettingsToUI();
                SaveSettingsToFile();
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Hiển thị thông báo
        /// </summary>
        private void ShowMessage(string message, string title, MessageBoxImage icon)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, icon);
        }

        /// <summary>
        /// Xuất cài đặt ra file
        /// </summary>
        public void ExportSettings(string filePath)
        {
            try
            {
                // Chỉ xuất các thuộc tính cần thiết
                var settingsToExport = new
                {
                    StartWithWindows = currentSettings.StartWithWindows,
                    NotifyNewDevice = currentSettings.NotifyNewDevice,
                    NotifyDisconnect = currentSettings.NotifyDisconnect,
                    NotifyUnknownMAC = currentSettings.NotifyUnknownMAC,
                    DeviceThreshold = currentSettings.DeviceThreshold,
                    ScanInterval = currentSettings.ScanInterval,
                    ExportedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var json = JsonSerializer.Serialize(settingsToExport, options);
                File.WriteAllText(filePath, json);

                ShowMessage("Xuất cài đặt thành công!", "Thông báo", MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowMessage($"Lỗi khi xuất cài đặt: {ex.Message}", "Lỗi", MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Nhập cài đặt từ file
        /// </summary>
        public void ImportSettings(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var importedSettings = JsonSerializer.Deserialize<SettingsData>(json);

                    if (importedSettings != null && importedSettings.IsValid())
                    {
                        currentSettings = importedSettings;
                        ApplySettingsToUI();
                        SaveSettingsToFile();

                        ShowMessage("Nhập cài đặt thành công!", "Thông báo", MessageBoxImage.Information);
                        OnSettingsChanged?.Invoke(currentSettings);
                    }
                    else
                    {
                        ShowMessage("File cài đặt không hợp lệ!", "Lỗi", MessageBoxImage.Error);
                    }
                }
                else
                {
                    ShowMessage("File không tồn tại!", "Lỗi", MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Lỗi khi nhập cài đặt: {ex.Message}", "Lỗi", MessageBoxImage.Error);
            }
        }
        #endregion
    }
}