using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace LANSPYproject
{
    public partial class NotificationPopup : Window
    {
        private DispatcherTimer autoCloseTimer;

        public NotificationPopup()
        {
            InitializeComponent();

            // Tự động đóng sau 5 giây
            autoCloseTimer = new DispatcherTimer();
            autoCloseTimer.Interval = TimeSpan.FromSeconds(5);
            autoCloseTimer.Tick += (s, e) =>
            {
                autoCloseTimer.Stop();
                this.Close();
            };
            autoCloseTimer.Start();
        }

        /// <summary>
        /// Hiển thị thông báo thiết bị mới
        /// </summary>
        /// <param name="deviceIP">Địa chỉ IP thiết bị</param>
        /// <param name="deviceName">Tên thiết bị</param>
        /// <param name="deviceMAC">Địa chỉ MAC thiết bị</param>
        /// <param name="owner">Cửa sổ cha</param>
        public static void ShowNewDeviceNotification(string deviceIP, string deviceName, string deviceMAC, Window owner = null)
        {
            var popup = new NotificationPopup();

            // Thiết lập giao diện cho thiết bị mới
            popup.HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(76, 175, 80)); // Xanh lá
            popup.IconText.Text = "+";
            popup.IconText.Foreground = new SolidColorBrush(Color.FromRgb(76, 175, 80));
            popup.TitleText.Text = "Thiết bị mới được phát hiện";

            // Thiết lập nội dung
            popup.DeviceInfoText.Text = $"Tên: {(string.IsNullOrEmpty(deviceName) ? "Không rõ" : deviceName)}";
            popup.IPAddressText.Text = $"IP: {(string.IsNullOrEmpty(deviceIP) ? "Unknown" : deviceIP)} | MAC: {(string.IsNullOrEmpty(deviceMAC) ? "Không rõ" : deviceMAC)}";
            popup.TimestampText.Text = $"Thời gian: {DateTime.Now:HH:mm:ss dd/MM/yyyy}";

            // Thiết lập vị trí hiển thị
            popup.SetWindowPosition(owner);
            popup.Show();
        }

        /// <summary>
        /// Hiển thị thông báo thiết bị offline
        /// </summary>
        /// <param name="deviceIP">Địa chỉ IP thiết bị</param>
        /// <param name="deviceName">Tên thiết bị</param>
        /// <param name="deviceMAC">Địa chỉ MAC thiết bị</param>
        /// <param name="owner">Cửa sổ cha</param>
        public static void ShowDeviceOfflineNotification(string deviceIP, string deviceName, string deviceMAC, Window owner = null)
        {
            var popup = new NotificationPopup();

            // Thiết lập giao diện cho thiết bị offline
            popup.HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(255, 87, 34)); // Đỏ cam
            popup.IconText.Text = "!";
            popup.IconText.Foreground = new SolidColorBrush(Color.FromRgb(255, 87, 34));
            popup.TitleText.Text = "Thiết bị mất kết nối";

            // Thiết lập nội dung
            popup.DeviceInfoText.Text = $"Tên: {(string.IsNullOrEmpty(deviceName) ? "Không rõ" : deviceName)}";
            popup.IPAddressText.Text = $"IP: {(string.IsNullOrEmpty(deviceIP) ? "Unknown" : deviceIP)} | MAC: {(string.IsNullOrEmpty(deviceMAC) ? "Không rõ" : deviceMAC)}";
            popup.TimestampText.Text = $"Thời gian: {DateTime.Now:HH:mm:ss dd/MM/yyyy}";

            // Thiết lập vị trí hiển thị
            popup.SetWindowPosition(owner);
            popup.Show();
        }


        /// <summary>
        /// Thiết lập vị trí hiển thị popup
        /// </summary>
        /// <param name="owner">Cửa sổ cha</param>
        private void SetWindowPosition(Window owner)
        {
            if (owner != null)
            {
                this.Owner = owner;

                // Hiển thị ở góc phải dưới của cửa sổ chính
                this.Left = owner.Left + owner.Width - this.Width - 20;
                this.Top = owner.Top + owner.Height - this.Height - 60;
            }
            else
            {
                // Hiển thị ở góc phải dưới màn hình
                this.Left = SystemParameters.WorkArea.Right - this.Width - 20;
                this.Top = SystemParameters.WorkArea.Bottom - this.Height - 20;
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút đóng
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            autoCloseTimer?.Stop();
            this.Close();
        }

        /// <summary>
        /// Dừng timer khi cửa sổ đóng
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            autoCloseTimer?.Stop();
            base.OnClosed(e);
        }
    }
}