using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Wpf;
using System.Diagnostics;

namespace LANSPYproject
{
    public partial class Dashboard : UserControl, INotifyPropertyChanged
    {
        public ChartValues<double> DownloadSpeeds { get; set; } = new ChartValues<double>();
        public ChartValues<double> UploadSpeeds { get; set; } = new ChartValues<double>();

        public SeriesCollection ChartSeries { get; set; }

        private List<DateTime> Timestamps { get; set; } = new List<DateTime>();

        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }

        private int onlineDevicesCount = 0;
        public int OnlineDevicesCount
        {
            get => onlineDevicesCount;
            set { onlineDevicesCount = value; OnPropertyChanged(); }
        }

        private int totalDevicesCount = 0;
        public int TotalDevicesCount
        {
            get => totalDevicesCount;
            set { totalDevicesCount = value; OnPropertyChanged(); }
        }

        private string wifiSSID = "Unknown";
        public string WifiSSID
        {
            get => wifiSSID;
            set { wifiSSID = value; OnPropertyChanged(); }
        }

        private string wifiBSSID = "Unknown";
        public string WifiBSSID
        {
            get => wifiBSSID;
            set { wifiBSSID = value; OnPropertyChanged(); }
        }

        private string wifiSpeed = "0 Mbps";
        public string WifiSpeed
        {
            get => wifiSpeed;
            set { wifiSpeed = value; OnPropertyChanged(); }
        }

        private string uploadSpeed = "0 KB/s";
        public string UploadSpeed
        {
            get => uploadSpeed;
            set { uploadSpeed = value; OnPropertyChanged(); }
        }

        private string downloadSpeed = "0 KB/s";
        public string DownloadSpeed
        {
            get => downloadSpeed;
            set { downloadSpeed = value; OnPropertyChanged(); }
        }

        private string wifiLatency = "N/A";
        public string WifiLatency
        {
            get => wifiLatency;
            set { wifiLatency = value; OnPropertyChanged(); }
        }

        private string lastScanTime = "Chưa quét";
        public string LastScanTime
        {
            get => lastScanTime;
            set { lastScanTime = value; OnPropertyChanged(); }
        }

        private int strangeDeviceAlerts = 0;
        public int StrangeDeviceAlerts
        {
            get => strangeDeviceAlerts;
            set { strangeDeviceAlerts = value; OnPropertyChanged(); }
        }

        public List<NetworkConnectionDevice> Devices { get; set; } = new List<NetworkConnectionDevice>();

        private DispatcherTimer updateTimer;

        public Dashboard()
        {
            InitializeComponent();
            DataContext = this;

            ChartSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Download",
                    Values = DownloadSpeeds,
                    StrokeThickness = 3,
                    PointGeometrySize = 5,
                    LineSmoothness = 0.5
                },
                new LineSeries
                {
                    Title = "Upload",
                    Values = UploadSpeeds,
                    StrokeThickness = 3,
                    PointGeometrySize = 5,
                    LineSmoothness = 0.5
                }
            };

            YFormatter = value => value.ToString("N2") + " KB/s";

            XFormatter = value =>
            {
                int index = (int)Math.Round(value);
                if (index >= 0 && index < Timestamps.Count)
                    return Timestamps[index].ToString("HH:mm:ss");
                return "";
            };

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromSeconds(30);
            updateTimer.Tick += async (s, e) => await UpdateNetworkInfoAsync();
            updateTimer.Start();

            _ = UpdateNetworkInfoAsync();
        }

        private async void UpdateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await UpdateNetworkInfoAsync();
        }

        private async Task UpdateNetworkInfoAsync()
        {
            LoadScannerData();

            WifiSSID = await GetCurrentWifiSSIDAsync();
            WifiBSSID = await GetCurrentWifiBSSIDAsync();
            WifiSpeed = await GetCurrentWifiSpeedAsync();
            await UpdateNetworkTrafficAsync();
            WifiLatency = await GetWifiLatencyAsync();
        }

        private void LoadScannerData()
        {
            OnlineDevicesCount = Devices.Count(d => d.IsOnline);
            TotalDevicesCount = Devices.Count;
            StrangeDeviceAlerts = Devices.Count(d => d.IsStrangeDevice);
            LastScanTime = "10 phút trước";
        }

        private async Task<string> GetCurrentWifiSSIDAsync()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var psi = new ProcessStartInfo("netsh", "wlan show interfaces")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using var process = Process.Start(psi);
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var regex = new Regex(@"^\s*SSID\s*:\s*(.+)$", RegexOptions.Multiline);
                    var match = regex.Match(output);
                    if (match.Success)
                    {
                        var ssid = match.Groups[1].Value.Trim();
                        if (ssid.Equals("BSSID", StringComparison.OrdinalIgnoreCase))
                            return "Unknown";
                        return ssid;
                    }
                    return "Unknown";
                });
            }
            catch
            {
                return "Unknown";
            }
        }

        private async Task<string> GetCurrentWifiBSSIDAsync()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var psi = new ProcessStartInfo("netsh", "wlan show interfaces")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using var process = Process.Start(psi);
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var regex = new Regex(@"^\s*BSSID\s*:\s*(.+)$", RegexOptions.Multiline);
                    var match = regex.Match(output);
                    if (match.Success)
                    {
                        return match.Groups[1].Value.Trim();
                    }
                    return "Unknown";
                });
            }
            catch
            {
                return "Unknown";
            }
        }

        private async Task<string> GetCurrentWifiSpeedAsync()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var psi = new ProcessStartInfo("netsh", "wlan show interfaces")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using var process = Process.Start(psi);
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var regex = new Regex(@"^\s*Receive rate \(Mbps\)\s*:\s*(.+)$", RegexOptions.Multiline);
                    var match = regex.Match(output);
                    if (match.Success)
                    {
                        return match.Groups[1].Value.Trim() + " Mbps";
                    }
                    return "0 Mbps";
                });
            }
            catch
            {
                return "0 Mbps";
            }
        }

        private async Task UpdateNetworkTrafficAsync()
        {
            var nic = NetworkInterface.GetAllNetworkInterfaces()
                        .Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && i.OperationalStatus == OperationalStatus.Up)
                        .FirstOrDefault();

            if (nic == null)
            {
                UploadSpeed = "0 KB/s";
                DownloadSpeed = "0 KB/s";
                return;
            }

            var stats1 = nic.GetIPv4Statistics();
            long bytesSent1 = stats1.BytesSent;
            long bytesReceived1 = stats1.BytesReceived;

            await Task.Delay(1000);

            var stats2 = nic.GetIPv4Statistics();
            long bytesSent2 = stats2.BytesSent;
            long bytesReceived2 = stats2.BytesReceived;

            long uploadBytesPerSec = bytesSent2 - bytesSent1;
            long downloadBytesPerSec = bytesReceived2 - bytesReceived1;

            UploadSpeed = $"{(uploadBytesPerSec / 1024.0):F2} KB/s";
            DownloadSpeed = $"{(downloadBytesPerSec / 1024.0):F2} KB/s";

            // Thêm timestamp hiện tại
            Timestamps.Add(DateTime.Now);

            // Giới hạn số điểm biểu đồ (ví dụ tối đa 30 điểm)
            int maxPoints = 30;
            if (DownloadSpeeds.Count >= maxPoints)
            {
                DownloadSpeeds.RemoveAt(0);
                UploadSpeeds.RemoveAt(0);
                Timestamps.RemoveAt(0);
            }

            DownloadSpeeds.Add(Math.Round(downloadBytesPerSec / 1024.0, 2));
            UploadSpeeds.Add(Math.Round(uploadBytesPerSec / 1024.0, 2));
        }

        private async Task<string> GetWifiLatencyAsync()
        {
            try
            {
                var gateway = GetDefaultGateway();
                if (string.IsNullOrEmpty(gateway))
                    gateway = "8.8.8.8";

                using Ping ping = new Ping();
                var reply = await ping.SendPingAsync(gateway, 1000);
                if (reply.Status == IPStatus.Success)
                {
                    return $"{reply.RoundtripTime} ms";
                }
            }
            catch { }
            return "N/A";
        }

        private string GetDefaultGateway()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var ni in interfaces)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.OperationalStatus == OperationalStatus.Up)
                {
                    var ipProps = ni.GetIPProperties();
                    foreach (var gateway in ipProps.GatewayAddresses)
                    {
                        if (gateway.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            return gateway.Address.ToString();
                    }
                }
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

   
}
