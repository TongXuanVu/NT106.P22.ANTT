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
    /// Interaction logic for Scanner.xaml
    /// </summary>
    public partial class Scanner : UserControl
    {
        public class Device
        {
            public int ID { get; set; }
            public string IP { get; set; }
            public string MAC { get; set; }
            public string Name { get; set; }
            // Date sẽ luôn được set theo thời gian hiện tại khi tạo mới
            public string Date { get; set; } = DateTime.Now.ToString("dd/MM, hh:mm tt");
        }

        public Scanner()
        {
            InitializeComponent();

            List<Device> devices = new List<Device>
        {
            new Device { ID = 1, IP = "192.168.1.10", MAC = "2C:54:91:88:C9:E3", Name = "hp el g4", Date = "25/03, 10:40 PM" },
            new Device { ID = 2, IP = "192.168.1.8", MAC = "D0:38:DC:3C:5B:48", Name = "Thinkpad x240", Date = "25/03, 10:40 PM" }
        };
            
            //deviceDataGrid.ItemsSource = devices;
        }
    }

}
