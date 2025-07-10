using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;

namespace LANSPYproject
{
    public partial class Alerts : UserControl
    {
        public class AlertItem
        {
            public string ThoiGian { get; set; }
            public string Loai { get; set; }
            public string NoiDung { get; set; }
        }

        public ObservableCollection<AlertItem> AlertItems { get; set; } = new ObservableCollection<AlertItem>();
        private ObservableCollection<AlertItem> AllAlerts { get; set; } = new ObservableCollection<AlertItem>();

        public Alerts()
        {
            InitializeComponent();
            AlertDataGrid.ItemsSource = AlertItems;
            RefreshAlerts();
        }

        public void AddAlert(string loai, string noiDung)
        {
            var item = new AlertItem
            {
                ThoiGian = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"),
                Loai = loai,
                NoiDung = noiDung
            };
            AlertItems.Insert(0, item);
            AllAlerts.Insert(0, item);
        }

        private void RefreshAlerts()
        {
            AlertItems.Clear();
            foreach (var alert in AllAlerts)
                AlertItems.Add(alert);
        }

        // 2. XÓA CẢNH BÁO CŨ (>7 ngày) hoặc có thể theo giờ phút
        private void DeleteOldAlertsButton_Click(object sender, RoutedEventArgs e)
        {
            var cutoff = DateTime.Now.AddMinutes(-1);
            var validAlerts = AllAlerts.Where(alert =>
            {
                if (DateTime.TryParseExact(alert.ThoiGian, "HH:mm:ss dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dt))
                {
                    return dt >= cutoff;
                }
                return true; // Nếu không parse được thì giữ lại cho chắc
            }).ToList();

            int removedCount = AllAlerts.Count - validAlerts.Count;
            AllAlerts = new ObservableCollection<AlertItem>(validAlerts);
            RefreshAlerts();

            MessageBox.Show($"Đã xóa {removedCount} cảnh báo cũ thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // 3. XUẤT BÁO CÁO (Excel)
        private void ExportAlertsButton_Click(object sender, RoutedEventArgs e)
        {
            if (AlertItems.Count == 0)
            {
                MessageBox.Show("Không có cảnh báo để xuất báo cáo.");
                return;
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Alerts");
                worksheet.Cell(1, 1).Value = "Thời gian";
                worksheet.Cell(1, 2).Value = "Loại";
                worksheet.Cell(1, 3).Value = "Nội dung";
                int row = 2;
                foreach (var alert in AlertItems)
                {
                    worksheet.Cell(row, 1).Value = alert.ThoiGian;
                    worksheet.Cell(row, 2).Value = alert.Loai;
                    worksheet.Cell(row, 3).Value = alert.NoiDung;
                    row++;
                }
                worksheet.Columns().AdjustToContents();

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Workbook|*.xlsx",
                    Title = "Lưu báo cáo",
                    FileName = "AlertsReport.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Xuất báo cáo thành công!");
                }
            }
        }
    }
}
