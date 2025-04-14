using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LANSPYproject
{
    /// <summary>
    /// Interaction logic for Alerts.xaml
    /// </summary>
    public partial class Alerts : Window
    {
        public class AlertItem
        {
            public string ThoiGian { get; set; }
            public string Loai { get; set; }
            public string NoiDung { get; set; }
        }
        public Alerts()
        {
            InitializeComponent();
            AlertDataGrid.ItemsSource = new List<AlertItem>
        {
            new AlertItem { ThoiGian = "11:22:10 10/04/2025", Loai = "Thông thường", NoiDung = "Thiết bị mới: 192.168.1.105" },
            new AlertItem { ThoiGian = "11:24:03 10/04/2025", Loai = "Lỗi nghiêm trọng", NoiDung = "Mất kết nối: 192.168.1.107" },
            new AlertItem { ThoiGian = "11:25:30 10/04/2025", Loai = "Thông thường", NoiDung = "Thiết bị lạ: MAC AA-BB-CC-DD-EE-FF" },
            new AlertItem { ThoiGian = "11:26:45 10/04/2025", Loai = "Lỗi nghiêm trọng", NoiDung = "Không ping được: 192.168.1.110" },
            new AlertItem { ThoiGian = "11:28:12 10/04/2025", Loai = "Thông thường", NoiDung = "Quá tải: 52 thiết bị hoạt động" },
        };
        }
    }

}
