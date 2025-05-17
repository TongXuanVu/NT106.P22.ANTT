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

        private static readonly Dictionary<string, string> OUIManufacturers = new Dictionary<string, string>
        {
            {"00:1A:2B", "Cisco Systems"},
            {"00:1B:44", "Dell Inc."},
            {"00:1C:B3", "Apple, Inc."},
            {"00:1D:7E", "Samsung Electronics"},
            // Thêm mã OUI nếu cần
        };

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

        public Scanner()
        {
            InitializeComponent();
            deviceDataGrid.ItemsSource = Devices;

            UpdateCurrentNetworkRange();

            NetworkChange.NetworkAddressChanged += (s, e) =>
            {
                Dispatcher.Invoke(() => UpdateCurrentNetworkRange());
            };

            this.DataContext = this;
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
                ID = 0,
                IP = localIP,
                MAC = mac,
                HostName = Dns.GetHostName(),
                Manufacturer = "Local Machine",
                ScanDate = DateTime.Now.ToString("dd/MM, hh:mm tt"),
                IsOn = true
            });
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
                Devices[i].ID = i;
            }
        }

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

            var semaphore = new SemaphoreSlim(50);
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
                            var reply = await ping.SendPingAsync(ip, 500);
                            if (reply.Status == IPStatus.Success)
                            {
                                NetworkDevice? device = null;
                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    device = Devices.FirstOrDefault(d => d.IP == ip);
                                    if (device == null)
                                    {
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
                                }
                            }
                            else
                            {
                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    var device = Devices.FirstOrDefault(d => d.IP == ip);
                                    if (device != null)
                                    {
                                        device.IsOn = false;
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
                            if (device != null)
                            {
                                device.IsOn = false;
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

            while (semaphore.CurrentCount < 50)
            {
                await Task.Delay(100, token);
            }

            token.ThrowIfCancellationRequested();
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

        private string GetManufacturerFromMac(string mac)
        {
            if (string.IsNullOrWhiteSpace(mac) || mac.Length < 8) return "Unknown";

            string oui = mac.Substring(0, 8).ToUpper().Replace('-', ':');

            if (OUIManufacturers.TryGetValue(oui, out var manufacturer))
                return manufacturer;

            return "Unknown";
        }

        private async Task UpdateNameForDevice(string ip, NetworkDevice device)
        {
            await Task.Run(() =>
            {
                try
                {
                    var entry = Dns.GetHostEntry(ip);
                    string hostname = entry.HostName;
                    if (!string.IsNullOrWhiteSpace(hostname))
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            device.HostName = hostname;
                        });
                    }
                }
                catch
                {
                    // giữ nguyên nếu lỗi
                }
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
            if (!string.IsNullOrEmpty(HostName) && !string.IsNullOrEmpty(Manufacturer))
                Name = $"{HostName} ({Manufacturer})";
            else if (!string.IsNullOrEmpty(HostName))
                Name = HostName;
            else if (!string.IsNullOrEmpty(Manufacturer))
                Name = Manufacturer;
            else
                Name = "Unknown";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
