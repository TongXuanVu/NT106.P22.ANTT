using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LANSPYproject
{
    public partial class Scanner : UserControl
    {
        public ObservableCollection<Device> Devices { get; set; } = new ObservableCollection<Device>();

        public Scanner()
        {
            InitializeComponent();
            deviceDataGrid.ItemsSource = Devices;
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đã nhấn");
            Devices.Clear();
            ScanNetworkAsync();
        }

        private async void ScanNetworkAsync()
        {
            string fullIP = GetLocalBaseIP();
            if (string.IsNullOrEmpty(fullIP) || !Regex.IsMatch(fullIP, @"^\d+\.\d+\.\d+\.\d+$"))
            {
                MessageBox.Show("Không tìm thấy IP hợp lệ!");
                return;
            }
            MessageBox.Show(fullIP);
            string baseIP = string.Join(".", fullIP.Split('.').Take(3)) + ".";
            var pingTasks = new List<Task>();
            var semaphore = new SemaphoreSlim(10); // Giới hạn số ping đồng thời

            for (int i = 1; i < 255; i++)
            {
                string ip = baseIP + i;
                await semaphore.WaitAsync();
                pingTasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        await new Ping().SendPingAsync(ip, 300);
                    }
                    catch { }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(pingTasks);
            await Task.Delay(1000); // Chờ bảng ARP cập nhật

            GetDevicesFromArp();
            UpdateHostNamesAsync(); // Cập nhật tên thiết bị nền
        }

        private void GetDevicesFromArp()
        {
            var arp = new ProcessStartInfo
            {
                FileName = "arp",
                Arguments = "-a",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = Process.Start(arp);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();

            var lines = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int id = 1;

            string fullIP = GetLocalBaseIP();
            if (string.IsNullOrEmpty(fullIP) || !Regex.IsMatch(fullIP, @"^\d+\.\d+\.\d+\.\d+$"))
                return;

            string baseIP = string.Join(".", fullIP.Split('.').Take(3)) + ".";

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"(\d+\.\d+\.\d+\.\d+)\s+([a-fA-F0-9:-]{17})");
                if (match.Success)
                {
                    string ip = match.Groups[1].Value;
                    string mac = match.Groups[2].Value;

                    // 🔴 Bỏ IP không cùng dải mạng (multicast, broadcast, khác subnet)
                    if (!ip.StartsWith(baseIP)) continue;
                    if (mac == "---" || mac == "ff-ff-ff-ff-ff-ff") continue;

                    Devices.Add(new Device
                    {
                        ID = id++,
                        IP = ip,
                        MAC = mac,
                        Name = "Unknow",
                        Date = DateTime.Now.ToString("dd/MM, hh:mm tt")
                    });
                }
            }
        }

        private async void UpdateHostNamesAsync()
        {
            foreach (var device in Devices)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        var entry = System.Net.Dns.GetHostEntry(device.IP);
                        string hostname = entry.HostName;

                        Dispatcher.Invoke(() =>
                        {
                            device.Name = hostname;
                        });
                    }
                    catch { }
                });

                await Task.Delay(100); // để tránh quá tải DNS
            }
        }

        private string GetLocalBaseIP()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Chỉ chọn adapter Wi-Fi đang hoạt động
                if (ni.OperationalStatus == OperationalStatus.Up &&
                    ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }

            return null;
        }

    }
}
