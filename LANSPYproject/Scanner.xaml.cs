using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
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

        private string GetHostName(string ip)
        {
            try
            {
                var entry = System.Net.Dns.GetHostEntry(ip);
                return entry.HostName;
            }
            catch
            {
                return "Unknown";
            }
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đã nhấn");
            Devices.Clear();
            ScanNetworkAsync();
        }

        private async void ScanNetworkAsync()
        {
            string baseIP = "192.168.1.";
            var pingTasks = new List<Task>();
            var semaphore = new SemaphoreSlim(20);

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
            await Task.Delay(1000); 

            GetDevicesFromArp();
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
                        Name = GetHostName(ip),
                        Date = DateTime.Now.ToString("dd/MM, hh:mm tt")
                    });
                }
            }
        }
    }
}
