using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Alerts : UserControl
    {
        public class AlertItem
        {
            public string ThoiGian { get; set; }
            public string Loai { get; set; }
            public string NoiDung { get; set; }
        }

        public ObservableCollection<AlertItem> AlertItems { get; set; } = new ObservableCollection<AlertItem>();

        public Alerts()
        {
            InitializeComponent();
            AlertDataGrid.ItemsSource = AlertItems;
        }

        public void AddAlert(string loai, string noiDung)
        {
            AlertItems.Insert(0, new AlertItem
            {
                ThoiGian = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"),
                Loai = loai,
                NoiDung = noiDung
            });
        }
    }


}
