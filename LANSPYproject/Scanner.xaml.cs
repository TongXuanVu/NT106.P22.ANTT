using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
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
            MessageBox.Show("Đã bấm nút quét!");
            Devices.Clear();
            ScanNetwork();
        }

        private async void ScanNetwork()
        {
            string baseIP = "192.168.1.";

            for (int i = 1; i < 255; i++)
            {
                string ip = baseIP + i;
                try
                {
                    await new Ping().SendPingAsync(ip, 300);
                }
                catch { }
            }

            await Task.Delay(2000); // Đợi bảng ARP cập nhật

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

            var lines = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int id = 1;
            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"(\d+\.\d+\.\d+\.\d+)\s+([a-fA-F0-9:-]{17})");
                if (match.Success)
                {
                    string ip = match.Groups[1].Value;
                    string mac = match.Groups[2].Value;
                    Devices.Add(new Device
                    {
                        ID = id++,
                        IP = ip,
                        MAC = mac,
                        Name = "Unknown",
                        Date = DateTime.Now.ToString("dd/MM, hh:mm tt")
                    });
                }
            }
        }

    }
}
