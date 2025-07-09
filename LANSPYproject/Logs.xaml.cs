using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using MySql.Data.MySqlClient;
namespace LANSPYproject
{
    public partial class Logs : UserControl
    {
        // Bộ dữ liệu gốc (toàn bộ thiết bị đã nhận từ Scanner)
        private ObservableCollection<NetworkDevice> allDevices = new ObservableCollection<NetworkDevice>();

        // Bộ dữ liệu hiển thị lên UI (có thể bị lọc)
        private ObservableCollection<NetworkDevice> displayedDevices = new ObservableCollection<NetworkDevice>();

        public Logs()
        {
            InitializeComponent();
            // Gán DataGrid với bộ dữ liệu hiển thị
            LogsDataGrid.ItemsSource = displayedDevices;
            LoadDevicesFromDatabase();
        }

        // Phương thức để Scanner gọi cập nhật dữ liệu mới sang Logs
        public void UpdateDevices(ObservableCollection<NetworkDevice> devices)
        {
            // Tải lại từ DB để giữ toàn bộ lịch sử
            LoadDevicesFromDatabase();

            // Thêm thiết bị mới vào nếu chưa có trong danh sách
            foreach (var device in devices)
            {
                bool exists = allDevices.Any(d => d.IP == device.IP && d.ScanDate == device.ScanDate);
                if (!exists)
                {
                    allDevices.Add(device);
                }
            }

            RefreshDisplayDevices();
        }


        // Hiển thị tất cả thiết bị hiện có (hoặc sau khi làm mới)
        private void RefreshDisplayDevices()
        {
            displayedDevices.Clear();
            foreach (var device in allDevices)
                displayedDevices.Add(device);

            UpdateStats();
        }

        // Cập nhật tổng số log và thiết bị lạ trên giao diện
        private void UpdateStats()
        {
            TotalLogsTextBlock.Text = $"Tổng số log: {displayedDevices.Count}";
            StrangeDevicesTextBlock.Text = $"Thiết bị lạ: {displayedDevices.Count(d => string.IsNullOrEmpty(d.Manufacturer) || d.Manufacturer == "Unknown")}";
        }

        // Lọc thiết bị theo các điều kiện tìm kiếm từ giao diện
        private bool TryParseDate(string dateStr, out DateTime date)
        {
            // Thử các format thông dụng
            string[] formats = { "dd/MM/yyyy HH:mm", "yyyy-MM-dd HH:mm:ss", "dd/MM/yyyy", "yyyy-MM-dd" };
            return DateTime.TryParseExact(dateStr, formats, null, System.Globalization.DateTimeStyles.None, out date);
        }

        private void FilterDevices(string ip, string mac, string deviceName, DateTime? from, DateTime? to)
        {
            var filtered = allDevices.Where(d =>
            {
                if (!string.IsNullOrWhiteSpace(ip) && !d.IP.Contains(ip)) return false;
                if (!string.IsNullOrWhiteSpace(mac) && !d.MAC.Contains(mac)) return false;
                if (!string.IsNullOrWhiteSpace(deviceName) && (d.Name == null || !d.Name.Contains(deviceName))) return false;

                if (from.HasValue)
                {
                    if (!TryParseDate(d.ScanDate, out var scanDate) || scanDate < from.Value)
                        return false;
                }
                if (to.HasValue)
                {
                    if (!TryParseDate(d.ScanDate, out var scanDate) || scanDate > to.Value)
                        return false;
                }
                return true;
            });

            displayedDevices.Clear();
            foreach (var device in filtered)
                displayedDevices.Add(device);

            UpdateStats();
        }


        // Xử lý sự kiện khi nhấn nút Tìm kiếm
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDevicesFromDatabase(); // Load lại để đảm bảo lọc toàn bộ

            string ip = IPTextBox.Text.Trim();
            string mac = MACTexBox.Text.Trim();
            string deviceName = DeviceTextBox.Text.Trim();
            DateTime? from = FromDatePicker.SelectedDate;
            DateTime? to = ToDatePicker.SelectedDate;

            // Cộng thêm 1 ngày để bao phủ toàn bộ ngày to.Value
            DateTime? adjustedTo = to?.AddDays(1);

            var filtered = allDevices.Where(d =>
            {
                if (!string.IsNullOrWhiteSpace(ip) && !d.IP.Contains(ip)) return false;
                if (!string.IsNullOrWhiteSpace(mac) && !d.MAC.Contains(mac)) return false;
                if (!string.IsNullOrWhiteSpace(deviceName) && (d.Name == null || !d.Name.Contains(deviceName))) return false;

                if (!TryParseDate(d.ScanDate, out var scanDate))
                    return false;

                if (from.HasValue && scanDate < from.Value) return false;
                if (adjustedTo.HasValue && scanDate >= adjustedTo.Value) return false;

                return true;
            });

            displayedDevices.Clear();
            foreach (var device in filtered)
                displayedDevices.Add(device);

            UpdateStats();
        }


        // Xử lý sự kiện khi nhấn nút Làm mới (Clear)
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            IPTextBox.Text = "";
            MACTexBox.Text = "";
            DeviceTextBox.Text = "";
            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;

            //RefreshDisplayDevices();
            LoadDevicesFromDatabase();
        }

        // Xử lý sự kiện khi nhấn nút Xuất báo cáo (xuất file CSV)


        private void ExportReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (displayedDevices.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo.");
                return;
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Logs");

                // Header
                worksheet.Cell(1, 1).Value = "SL No";
                worksheet.Cell(1, 2).Value = "MAC Address";
                worksheet.Cell(1, 3).Value = "IP Address";
                worksheet.Cell(1, 4).Value = "Tên thiết bị";
                worksheet.Cell(1, 5).Value = "Thời gian";
                worksheet.Cell(1, 6).Value = "Ghi chú";

                int row = 2;
                foreach (var device in displayedDevices)
                {
                    worksheet.Cell(row, 1).Value = device.ID;
                    worksheet.Cell(row, 2).Value = device.MAC;
                    worksheet.Cell(row, 3).Value = device.IP;
                    worksheet.Cell(row, 4).Value = device.Name;
                    worksheet.Cell(row, 5).Value = device.ScanDate;
                    worksheet.Cell(row, 6).Value = device.Manufacturer; // hoặc trường nào bạn muốn
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Workbook|*.xlsx",
                    Title = "Lưu báo cáo",
                    FileName = "LogsReport.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Xuất báo cáo thành công!");
                }
            }
        }




        // Xử lý sự kiện khi nhấn nút Xóa log cũ (> 7 ngày)
        private void DeleteOldLogsButton_Click(object sender, RoutedEventArgs e)
        {
            var cutoffDate = DateTime.Now.AddDays(-7);

            // Lọc lại bộ dữ liệu gốc loại bỏ thiết bị cũ
            var filtered = allDevices.Where(d =>
                DateTime.TryParse(d.ScanDate, out var scanDate) && scanDate >= cutoffDate);

            allDevices = new ObservableCollection<NetworkDevice>(filtered);

            RefreshDisplayDevices();
        }

        private void LoadDevicesFromDatabase()
        {
            string connectionString = "server=localhost;user=root;password=1234;database=lan_spy_db;";
            string query = "SELECT ip, mac, name, scan_time, status FROM scanner_devices ORDER BY scan_time DESC";

            allDevices.Clear();

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int id = 0;
                    while (reader.Read())
                    {
                        string ip = reader["ip"]?.ToString() ?? "Unknown";
                        string mac = reader["mac"]?.ToString() ?? "Unknown";
                        string nameField = reader["name"]?.ToString() ?? "Unknown";
                        DateTime scanTime = Convert.ToDateTime(reader["scan_time"]);
                        bool isOnline = (reader["status"]?.ToString() ?? "").Equals("Online", StringComparison.OrdinalIgnoreCase);

                        var device = new NetworkDevice
                        {
                            ID = id++,
                            IP = ip,
                            MAC = mac,
                            ScanDate = scanTime.ToString("dd/MM/yyyy HH:mm"),
                            IsOn = isOnline
                        };

                        if (nameField.Contains("(") && nameField.Contains(")"))
                        {
                            var openIdx = nameField.IndexOf('(');
                            var closeIdx = nameField.IndexOf(')');
                            device.HostName = nameField.Substring(0, openIdx).Trim();
                            device.Manufacturer = nameField.Substring(openIdx + 1, closeIdx - openIdx - 1).Trim();
                        }
                        else
                        {
                            device.HostName = nameField;
                            device.Manufacturer = "Unknown";
                        }

                        allDevices.Add(device);
                    }
                }
            }

            RefreshDisplayDevices();
        }
    }
}
