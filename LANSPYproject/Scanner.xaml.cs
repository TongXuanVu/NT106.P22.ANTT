using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using System.IO;


namespace LANSPYproject
{
    public class BoolToOnlineOfflineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? "Online" : "Offline";
            return "Offline";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
                return s.Equals("Online", StringComparison.OrdinalIgnoreCase);
            return false;
        }
    }

    public class BoolToBrushConverter : IValueConverter
    {
        private static readonly Brush OnlineBrush = new SolidColorBrush(Color.FromRgb(0, 176, 80)); // xanh lá
        private static readonly Brush OfflineBrush = new SolidColorBrush(Color.FromRgb(255, 59, 48)); // đỏ

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? OnlineBrush : OfflineBrush;
            return OfflineBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public partial class Scanner : UserControl, INotifyPropertyChanged
    {
        public Logs? LogsControl { get; set; }
        public Alerts? AlertsControl { get; set; }

        private static Dictionary<string, string> OUIManufacturers = new();
        public event Action<DateTime>? ScanCompleted;
        public ObservableCollection<NetworkDevice> Devices { get; set; } = new ObservableCollection<NetworkDevice>();

        private CancellationTokenSource? cts = null;

        private string currentNetworkRange = "Không xác định";
        public string CurrentNetworkRange
        {
            get => currentNetworkRange;
            set
            {
                if (currentNetworkRange != value)
                {
                    currentNetworkRange = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool hasScannedOnce = false;

        // === Auto Scan Interval ===
        private DispatcherTimer? autoScanTimer;
        private SettingsData currentSettings = new SettingsData();

        public Scanner()
        {
            CreateDatabaseIfNotExists();
            InitializeComponent();
            deviceDataGrid.ItemsSource = Devices;
            UpdateCurrentNetworkRange();
            NetworkChange.NetworkAddressChanged += (s, e) =>
            {
                Dispatcher.Invoke(() => UpdateCurrentNetworkRange());
            };
            this.DataContext = this;
            string csvPath = "oui.csv";
            if (File.Exists(csvPath))
            {
                OUIManufacturers = LoadOUIFromCSV(csvPath);
            }

            SetupAutoScanTimer();
        }// tự động quét theo interval khi mở

        private static void CreateDatabaseIfNotExists()
        {
            string connectionString = "server=localhost;user=root;password=260805;";
            string dbName = "lan_spy_db";
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                // Tạo database nếu chưa có
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand($"CREATE DATABASE IF NOT EXISTS {dbName} CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            // Tạo bảng nếu chưa có
            string tableConnStr = connectionString + $"database={dbName};";
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(tableConnStr))
            {
                conn.Open();
                string createTableSql = @"CREATE TABLE IF NOT EXISTS scanner_devices (
            id INT AUTO_INCREMENT PRIMARY KEY,
            ip VARCHAR(64),
            mac VARCHAR(64),
            name VARCHAR(255),
            scan_time DATETIME,
            status VARCHAR(32),
            wifi_name VARCHAR(255)
        );";
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(createTableSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private static Dictionary<string, string> LoadOUIFromCSV(string csvPath)
        {
            var ouiDict = new Dictionary<string, string>();
            var lines = File.ReadAllLines(csvPath);

            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length >= 3)
                {
                    string rawOUI = parts[1].Replace("\"", "").Trim().ToUpper(); // ví dụ: "E00630"
                    string org = parts[2].Replace("\"", "").Trim();

                    if (!ouiDict.ContainsKey(rawOUI))
                        ouiDict[rawOUI] = org;
                }
            }

            return ouiDict;
        }



        // Nhận settings từ Setting control
        public void UpdateSettings(SettingsData newSettings)
        {
            if (newSettings != null && newSettings.IsValid())
            {
                currentSettings = newSettings.Clone();
                SetupAutoScanTimer();
            }
        }

        private void SetupAutoScanTimer()
        {
            if (autoScanTimer == null)
            {
                autoScanTimer = new DispatcherTimer();
                autoScanTimer.Tick += (s, e) =>
                {
                    // Không cho chạy đồng thời nhiều lần!
                    if (cts == null)
                        StartScanning();
                };
            }

            autoScanTimer.Stop();
            autoScanTimer.Interval = TimeSpan.FromSeconds(currentSettings.ScanInterval);

            // Nếu muốn tắt auto scan, chỉ cần comment dòng sau
            autoScanTimer.Start();
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            if (!hasScannedOnce)
            {
                Devices.Clear();
                AddLocalDevice();
                hasScannedOnce = true;

                ScanButton.Content = "Cập nhật";
            }

            UpdateCurrentNetworkRange();
            StartScanning();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopScanning();
        }

        private void UpdateCurrentNetworkRange()
        {
            var ipSubnetWiFi = GetWiFiIPAndSubnetMask();
            if (ipSubnetWiFi != null)
            {
                var (ip, subnet) = ipSubnetWiFi.Value;
                CurrentNetworkRange = GetNetworkAddress(ip, subnet) + "/" + GetSubnetMaskLength(subnet);
                return;
            }

            var ipSubnetEthernet = GetEthernetIPAndSubnetMask();
            if (ipSubnetEthernet != null)
            {
                var (ip, subnet) = ipSubnetEthernet.Value;
                CurrentNetworkRange = GetNetworkAddress(ip, subnet) + "/" + GetSubnetMaskLength(subnet);
                return;
            }

            CurrentNetworkRange = "Không có kết nối WiFi";
        }

        private (string ipAddress, string subnetMask)? GetWiFiIPAndSubnetMask()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                    ni.OperationalStatus == OperationalStatus.Up)
                {
                    var ipProps = ni.GetIPProperties();
                    if (ipProps.GatewayAddresses.Any(g => g.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        foreach (var unicast in ipProps.UnicastAddresses)
                        {
                            if (unicast.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                return (unicast.Address.ToString(), unicast.IPv4Mask.ToString());
                            }
                        }
                    }
                }
            }
            return null;
        }

        private (string ipAddress, string subnetMask)? GetEthernetIPAndSubnetMask()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    ni.OperationalStatus == OperationalStatus.Up)
                {
                    var ipProps = ni.GetIPProperties();
                    if (ipProps.GatewayAddresses.Any(g => g.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        foreach (var unicast in ipProps.UnicastAddresses)
                        {
                            if (unicast.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                return (unicast.Address.ToString(), unicast.IPv4Mask.ToString());
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void AddLocalDevice()
        {
            var localIPSubnet = GetWiFiIPAndSubnetMask() ?? GetEthernetIPAndSubnetMask();
            if (localIPSubnet == null) return;

            var (localIP, subnet) = localIPSubnet.Value;
            string? mac = GetLocalMacAddress();

            if (mac == null) mac = "Unknown";

            if (Devices.Any(d => d.IP == localIP)) return;

            Devices.Insert(0, new NetworkDevice
            {
                ID = 1, // Bắt đầu từ 1
                IP = localIP,
                MAC = mac,
                HostName = Dns.GetHostName(),
                Manufacturer = "Local Machine",
                ScanDate = DateTime.Now.ToString("dd/MM, hh:mm tt"),
                IsOn = true
            });
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

        private string? GetLocalMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up &&
                    (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                     nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                {
                    var macAddress = nic.GetPhysicalAddress();
                    if (macAddress != null && macAddress.GetAddressBytes().Length == 6)
                    {
                        return string.Join(":", macAddress.GetAddressBytes().Select(b => b.ToString("X2")));
                    }
                }
            }
            return null;
        }

        private async void StartScanning()
        {
            if (cts != null)
                return;

            cts = new CancellationTokenSource();

            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var device in Devices)
                {
                    device.IsOn = false;
                }
            });

            StatusTextBlock.Text = "Đang quét...";
            StatusTextBlock.Foreground = Brushes.Green;
            ScanButton.IsEnabled = false;
            StopButton.IsEnabled = true;

            try
            {
                await ScanNetworkAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                StatusTextBlock.Text = "Quét bị dừng";
                StatusTextBlock.Foreground = Brushes.OrangeRed;
            }
            finally
            {
                cts.Dispose();
                cts = null;

                if (StatusTextBlock.Text != "Quét bị dừng")
                {
                    StatusTextBlock.Text = "Quét hoàn thành";
                    StatusTextBlock.Foreground = Brushes.Blue;
                }

                ScanButton.IsEnabled = true;
                StopButton.IsEnabled = false;

                LogsControl?.UpdateDevices(Devices);

            }
        }

        private void StopScanning()
        {
            cts?.Cancel();
        }

        private void ReassignIDs()
        {
            for (int i = 0; i < Devices.Count; i++)
            {
                Devices[i].ID = i + 1; // Bắt đầu từ 1
            }
        }

        // Method ScanNetworkAsync đã điều chỉnh lấy thông số từ currentSettings
        private async Task ScanNetworkAsync(CancellationToken token)
        {
            var ipSubnet = GetWiFiIPAndSubnetMask() ?? GetEthernetIPAndSubnetMask();
            if (ipSubnet == null)
            {
                MessageBox.Show("Không tìm thấy IP hợp lệ!");
                return;
            }

            var (ipAddress, subnetMask) = ipSubnet.Value;
            var ipList = GetLimitedHosts(ipAddress);

            var semaphore = new SemaphoreSlim(currentSettings.DeviceThreshold > 0 ? currentSettings.DeviceThreshold : 50); // lấy DeviceThreshold làm max thread
            int scannedCount = 0;

            foreach (var ip in ipList)
            {
                token.ThrowIfCancellationRequested();
                await semaphore.WaitAsync(token);

                _ = Task.Run(async () =>
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        using (var ping = new Ping())
                        {
                            var reply = await ping.SendPingAsync(ip, 500); // (có thể bổ sung thời gian timeout từ settings)
                            if (reply.Status == IPStatus.Success)
                            {
                                NetworkDevice? device = null;
                                bool isNewDevice = false;

                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    device = Devices.FirstOrDefault(d => d.IP == ip);
                                    if (device == null)
                                    {
                                        isNewDevice = true;
                                        device = new NetworkDevice
                                        {
                                            ID = Devices.Count + 1,
                                            IP = ip,
                                            MAC = "Đang lấy...",
                                            HostName = "",
                                            Manufacturer = "",
                                            ScanDate = DateTime.Now.ToString("dd/MM, hh:mm tt"),
                                            IsOn = true
                                        };
                                        Devices.Add(device);
                                        AlertsControl?.AddAlert("Thông thường", $"Thiết bị mới: {ip}");
                                        ReassignIDs();
                                    }
                                    else
                                    {
                                        device.IsOn = true;
                                    }
                                });

                                if (device != null)
                                {
                                    await UpdateMacForDevice(ip, device);
                                    await UpdateNameForDevice(ip, device);
                                    SaveDeviceToDatabase(device);

                                    // Popup cảnh báo thiết bị mới nếu bật trong settings
                                    if (isNewDevice && currentSettings.NotifyNewDevice)
                                    {
                                        App.Current.Dispatcher.Invoke(() =>
                                        {
                                            var mainWindow = Application.Current.MainWindow;
                                            NotificationPopup.ShowNewDeviceNotification(
                                                device.IP,
                                                device.Name,
                                                device.MAC,
                                                mainWindow);
                                        });
                                    }
                                    // Popup thiết bị lạ (MAC unknown) nếu bật trong settings
                                    if (currentSettings.NotifyUnknownMAC && device.MAC == "Unknown")
                                    {
                                        App.Current.Dispatcher.Invoke(() =>
                                        {
                                            AlertsControl?.AddAlert("Thiết bị lạ", $"Thiết bị MAC chưa xác định: {device.IP}");
                                        });
                                    }
                                }
                            }
                            else
                            {
                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    var device = Devices.FirstOrDefault(d => d.IP == ip);
                                    if (device != null && device.IsOn)
                                    {
                                        device.IsOn = false;
                                        if (currentSettings.NotifyDisconnect)
                                        {
                                            AlertsControl?.AddAlert("Thông báo", $"Mất kết nối: {device.IP}");
                                            var mainWindow = Application.Current.MainWindow;
                                            NotificationPopup.ShowDeviceOfflineNotification(
                                                device.IP,
                                                device.Name,
                                                device.MAC,
                                                mainWindow);
                                        }
                                    }
                                });
                            }
                        }
                    }
                    catch
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            var device = Devices.FirstOrDefault(d => d.IP == ip);
                            if (device != null && device.IsOn)
                            {
                                device.IsOn = false;
                                if (currentSettings.NotifyDisconnect)
                                {
                                    var mainWindow = Application.Current.MainWindow;
                                    NotificationPopup.ShowDeviceOfflineNotification(
                                        device.IP,
                                        device.Name,
                                        device.MAC,
                                        mainWindow);
                                }
                            }
                        });
                    }
                    finally
                    {
                        semaphore.Release();
                        Interlocked.Increment(ref scannedCount);
                        if (scannedCount % 10 == 0)
                        {
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                StatusTextBlock.Text = $"Đang quét... ({scannedCount}/{ipList.Count})";
                            });
                        }
                    }
                }, token);

                await Task.Delay(30, token);
            }

            while (semaphore.CurrentCount < (currentSettings.DeviceThreshold > 0 ? currentSettings.DeviceThreshold : 50))
            {
                await Task.Delay(100, token);
            }

            token.ThrowIfCancellationRequested();
            ScanCompleted?.Invoke(DateTime.Now);
        }

        private List<string> GetLimitedHosts(string ipAddress)
        {
            var ips = new List<string>();
            try
            {
                var ipParts = ipAddress.Split('.');
                if (ipParts.Length != 4) return ips;

                string baseIp = $"{ipParts[0]}.{ipParts[1]}.{ipParts[2]}";

                for (int i = 1; i <= 254; i++)
                {
                    ips.Add($"{baseIp}.{i}");
                }
            }
            catch { }
            return ips;
        }

        private async Task UpdateMacForDevice(string ip, NetworkDevice device)
        {
            try
            {
                var arp = new ProcessStartInfo
                {
                    FileName = "arp",
                    Arguments = "-a " + ip,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var process = Process.Start(arp);
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Dispose();

                var regex = new Regex(@"(([a-fA-F0-9]{2}[:-]){5}[a-fA-F0-9]{2})");
                var match = regex.Match(output);
                if (match.Success)
                {
                    var mac = match.Value.ToUpper();
                    if (!string.IsNullOrWhiteSpace(mac))
                    {
                        string manufacturer = GetManufacturerFromMac(mac);
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            device.MAC = mac;
                            device.Manufacturer = manufacturer;
                        });
                    }
                }
            }
            catch { }
        }

        private async Task<string?> GetNetbiosName(string ip)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "nbtstat",
                        Arguments = "-A " + ip,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    var process = Process.Start(psi);
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    process.Dispose();

                    var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        if (line.Contains("<00>") && line.Contains("UNIQUE"))
                        {
                            var name = line.Trim().Split(' ')[0];
                            return name;
                        }
                    }
                }
                catch { }

                return null;
            });
        }

        private string GetManufacturerFromMac(string mac)
        {
            if (string.IsNullOrWhiteSpace(mac)) return "Unknown";

            // Loại bỏ dấu và chỉ lấy 6 byte đầu tiên
            string cleanMac = mac.Replace(":", "").Replace("-", "").ToUpper();
            if (cleanMac.Length < 6) return "Unknown";

            string ouiKey = cleanMac.Substring(0, 6); // ví dụ: "200889"

            if (OUIManufacturers.TryGetValue(ouiKey, out var manufacturer))
                return manufacturer;

            return "Unknown";
        }



        private async Task UpdateNameForDevice(string ip, NetworkDevice device)
        {
            string? hostName = null;

            // Bước 1: Thử lấy hostname từ DNS
            try
            {
                var entry = await Dns.GetHostEntryAsync(ip);
                if (!string.IsNullOrWhiteSpace(entry.HostName))
                {
                    hostName = entry.HostName;
                }
            }
            catch
            {
                // DNS không resolve được, thử NetBIOS
            }

            // Bước 2: Nếu DNS fail, thử NetBIOS
            if (string.IsNullOrWhiteSpace(hostName))
            {
                hostName = await GetNetbiosName(ip);
            }

            // Bước 3: Nếu vẫn null → đặt Unknown
            if (string.IsNullOrWhiteSpace(hostName))
            {
                hostName = "Unknown";
            }

            // Gán kết quả vào thiết bị
            App.Current.Dispatcher.Invoke(() =>
            {
                device.HostName = hostName;
            });
        }


        private string GetNetworkAddress(string ipAddress, string subnetMask)
        {
            var ip = IPAddress.Parse(ipAddress).GetAddressBytes();
            var mask = IPAddress.Parse(subnetMask).GetAddressBytes();

            byte[] network = new byte[ip.Length];
            for (int i = 0; i < ip.Length; i++)
                network[i] = (byte)(ip[i] & mask[i]);

            return new IPAddress(network).ToString();
        }

        private int GetSubnetMaskLength(string subnetMask)
        {
            var mask = IPAddress.Parse(subnetMask).GetAddressBytes();
            int length = 0;
            foreach (var b in mask)
            {
                for (int i = 7; i >= 0; i--)
                {
                    if ((b & (1 << i)) != 0) length++;
                    else break;
                }
            }
            return length;
        }

        private bool IsInSameSubnet(string ipAddress, string subnetAddress, string subnetMask)
        {
            var ip = IPAddress.Parse(ipAddress).GetAddressBytes();
            var subnet = IPAddress.Parse(subnetAddress).GetAddressBytes();
            var mask = IPAddress.Parse(subnetMask).GetAddressBytes();

            for (int i = 0; i < ip.Length; i++)
            {
                if ((ip[i] & mask[i]) != (subnet[i] & mask[i]))
                    return false;
            }
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void SaveDeviceToDatabase(NetworkDevice device)
        {
            string connectionString = "server=localhost;user=root;password=260805;database=lan_spy_db;";
            using (var conn = new MySqlConnection(connectionString))
            {
                // Kiểm tra trùng lặp theo IP và MAC
                string checkQuery = @"SELECT COUNT(*) FROM scanner_devices WHERE ip = @ip AND mac = @mac";
                using (var checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ip", device.IP ?? "Unknown");
                    checkCmd.Parameters.AddWithValue("@mac", device.MAC ?? "Unknown");
                    conn.Open();
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    conn.Close();
                    if (count > 0)
                    {
                        // Nếu đã có, cập nhật thông tin mới nhất
                        string updateQuery = @"UPDATE scanner_devices SET name = @name, scan_time = @scan_time, status = @status, wifi_name = @wifi_name WHERE ip = @ip AND mac = @mac";
                        using (var updateCmd = new MySqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@name", device.Name ?? "Unknown");
                            updateCmd.Parameters.AddWithValue("@scan_time", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("@status", device.IsOn ? "Online" : "Offline");
                            updateCmd.Parameters.AddWithValue("@wifi_name", GetWifiName() ?? "Unknown");
                            updateCmd.Parameters.AddWithValue("@ip", device.IP ?? "Unknown");
                            updateCmd.Parameters.AddWithValue("@mac", device.MAC ?? "Unknown");
                            conn.Open();
                            updateCmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        return;
                    }
                }

                // Nếu chưa có, thêm mới
                string query = @"INSERT INTO scanner_devices (ip, mac, name, scan_time, status, wifi_name)
                         VALUES (@ip, @mac, @name, @scan_time, @status, @wifi_name)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ip", device.IP ?? "Unknown");
                    cmd.Parameters.AddWithValue("@mac", device.MAC ?? "Unknown");
                    cmd.Parameters.AddWithValue("@name", device.Name ?? "Unknown");
                    cmd.Parameters.AddWithValue("@scan_time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@status", device.IsOn ? "Online" : "Offline");
                    cmd.Parameters.AddWithValue("@wifi_name", GetWifiName() ?? "Unknown");

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

    }

    public class NetworkDevice : INotifyPropertyChanged
    {
        private bool _isOn;
        public bool IsOn
        {
            get => _isOn;
            set { _isOn = value; OnPropertyChanged(); }
        }

        private int _id;
        private string _ip = "";
        private string _mac = "";
        private string _manufacturer = "";
        private string _hostName = "";
        private string _name = "";
        private string _scanDate = "";
        public string WifiName { get; set; }

        public int ID
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string IP
        {
            get => _ip;
            set { _ip = value; OnPropertyChanged(); }
        }

        public string MAC
        {
            get => _mac;
            set { _mac = value; OnPropertyChanged(); }
        }

        public string Manufacturer
        {
            get => _manufacturer;
            set
            {
                _manufacturer = value;
                UpdateNameCombined();
            }
        }

        public string HostName
        {
            get => _hostName;
            set
            {
                _hostName = value;
                UpdateNameCombined();
            }
        }

        public string Name
        {
            get => _name;
            private set { _name = value; OnPropertyChanged(); }
        }

        public string ScanDate
        {
            get => _scanDate;
            set { _scanDate = value; OnPropertyChanged(); }
        }

        private void UpdateNameCombined()
        {
            string guessedType = GuessDeviceTypeFromManufacturer(Manufacturer);

            bool hasManufacturer = !string.IsNullOrWhiteSpace(Manufacturer);
            bool hasGuess = !string.IsNullOrWhiteSpace(guessedType) && guessedType != "Unknown";

            if (hasManufacturer && hasGuess)
                Name = $"{Manufacturer} ({guessedType})";
            else if (hasManufacturer)
                Name = Manufacturer;
            else
                Name = "Unknown";
        }

        private string GuessDeviceTypeFromManufacturer(string manufacturer)
        {
            if (string.IsNullOrWhiteSpace(manufacturer)) return "Unknown";

            string m = manufacturer.ToLower();

            // === ROUTERS & NETWORKING ===
            if (m.Contains("tp-link") || m.Contains("tplink") || m.Contains("d-link") || m.Contains("netgear") ||
                m.Contains("tenda") || m.Contains("zyxel") || m.Contains("huawei") || m.Contains("zte") ||
                m.Contains("mikrotik") || m.Contains("fiberhome"))
                return "Home Router / Modem";

            if (m.Contains("cisco") || m.Contains("juniper") || m.Contains("aruba") || m.Contains("hpe") ||
                m.Contains("ubiquiti") || m.Contains("meraki") || m.Contains("ruckus"))
                return "Enterprise Network Device";

            // === CAMERAS ===
            if (m.Contains("hikvision") || m.Contains("dahua") || m.Contains("ezviz") || m.Contains("uniview") ||
                m.Contains("reolink") || m.Contains("axis") || m.Contains("vivotek") || m.Contains("amcrest"))
                return "IP Camera / Surveillance";

            // === COMPUTERS ===
            if (m.Contains("dell") || m.Contains("hp") || m.Contains("asus") || m.Contains("acer") ||
                m.Contains("msi") || m.Contains("lenovo") || m.Contains("gigabyte") || m.Contains("samsung electronics co.,ltd") ||
                m.Contains("system76") || m.Contains("apple") && m.Contains("mac"))
                return "Laptop / Desktop";

            if (m.Contains("intel") || m.Contains("amd") || m.Contains("nvidia"))
                return "Computer Component";

            // === MOBILE DEVICES ===
            if (m.Contains("apple") || m.Contains("samsung") || m.Contains("xiaomi") || m.Contains("oppo") ||
                m.Contains("vivo") || m.Contains("realme") || m.Contains("oneplus") || m.Contains("huawei") ||
                m.Contains("zte") || m.Contains("nokia") || m.Contains("infinix") || m.Contains("motorola"))
                return "Smartphone / Tablet";

            // === TV / MEDIA ===
            if (m.Contains("sony") || m.Contains("lg") || m.Contains("tcl") || m.Contains("hisense") ||
                m.Contains("panasonic") || m.Contains("philips") && m.Contains("tv"))
                return "Smart TV / Media Device";

            // === PRINTING ===
            if (m.Contains("canon") || m.Contains("epson") || m.Contains("brother") || m.Contains("lexmark") ||
                m.Contains("hp inc") || m.Contains("ricoh") || m.Contains("xerox"))
                return "Printer / Scanner";

            // === IOT / SMART HOME ===
            if (m.Contains("espressif") || m.Contains("tuya") || m.Contains("shelly") || m.Contains("sonoff") ||
                m.Contains("broadlink") || m.Contains("digoo") || m.Contains("gosund") || m.Contains("nspanel") ||
                m.Contains("tuyatec") || m.Contains("smartthings"))
                return "IoT / Smart Home Device";

            // === VIRTUAL SYSTEMS ===
            if (m.Contains("vmware") || m.Contains("virtualbox") || m.Contains("parallels") ||
                m.Contains("qemu") || m.Contains("xen"))
                return "Virtual Machine";

            // === GAMING / AUDIO ===
            if (m.Contains("nintendo") || m.Contains("playstation") || m.Contains("xbox"))
                return "Game Console";

            if (m.Contains("bose") || m.Contains("sonos") || m.Contains("jbl") || m.Contains("marshall"))
                return "Smart Audio Device";

            // === SPECIAL CASES ===
            if (m.Contains("fn-link") || m.Contains("hunan") || m.Contains("quectel") ||
                m.Contains("neoway") || m.Contains("simcom"))
                return "WiFi / Cellular Module";

            return "Unknown";
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


    }

}
