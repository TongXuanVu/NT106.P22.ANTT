using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
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
        }

        // Phương thức để Scanner gọi cập nhật dữ liệu mới sang Logs
        public void UpdateDevices(ObservableCollection<NetworkDevice> devices)
        {
            allDevices.Clear();
            foreach (var device in devices)
                allDevices.Add(device);

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
            // Lấy giá trị nhập từ các textbox và datepicker
            string ip = IPTextBox.Text.Trim();
            string mac = MACTexBox.Text.Trim();
            string deviceName = DeviceTextBox.Text.Trim();
            DateTime? from = FromDatePicker.SelectedDate;
            DateTime? to = ToDatePicker.SelectedDate;

            // Lọc danh sách allDevices theo các điều kiện nhập
            var filtered = allDevices.Where(d =>
                (string.IsNullOrWhiteSpace(ip) || d.IP.Contains(ip)) &&
                (string.IsNullOrWhiteSpace(mac) || d.MAC.Contains(mac)) &&
                (string.IsNullOrWhiteSpace(deviceName) || (d.Name != null && d.Name.Contains(deviceName))) &&
                (!from.HasValue || DateTime.TryParse(d.ScanDate, out var scanDate) && scanDate >= from.Value) &&
                (!to.HasValue || DateTime.TryParse(d.ScanDate, out scanDate) && scanDate <= to.Value)
            );

            // Xóa danh sách hiển thị cũ
            displayedDevices.Clear();

            // Thêm các thiết bị thỏa mãn điều kiện vào danh sách hiển thị
            foreach (var device in filtered)
            {
                displayedDevices.Add(device);
            }

            // Cập nhật lại thông tin thống kê (tổng số log, thiết bị lạ)
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

            RefreshDisplayDevices();
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
    }
}
