using System;

namespace LANSPYproject
{
    /// <summary>
    /// Class chứa tất cả các cài đặt của ứng dụng
    /// </summary>
    public class SettingsData
    {
        /// <summary>
        /// Khởi động cùng Windows
        /// </summary>
        public bool StartWithWindows { get; set; } = true;

        /// <summary>
        /// Gửi cảnh báo khi phát hiện thiết bị mới
        /// </summary>
        public bool NotifyNewDevice { get; set; } = false;

        /// <summary>
        /// Gửi cảnh báo khi thiết bị mất kết nối
        /// </summary>
        public bool NotifyDisconnect { get; set; } = true;

        /// <summary>
        /// Gửi cảnh báo khi có thiết bị lạ (MAC chưa rõ)
        /// </summary>
        public bool NotifyUnknownMAC { get; set; } = true;

        /// <summary>
        /// Ngưỡng cảnh báo số lượng thiết bị (mặc định: 50)
        /// </summary>
        public int DeviceThreshold { get; set; } = 50;

        /// <summary>
        /// Thời gian quét lại (giây) (mặc định: 300 giây = 5 phút)
        /// </summary>
        public int ScanInterval { get; set; } = 300;

        /// <summary>
        /// Thời gian tạo cài đặt (chỉ dành cho tracking, không lưu vào JSON)
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Thời gian cập nhật cài đặt lần cuối (chỉ dành cho tracking, không lưu vào JSON)
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        /// <summary>
        /// Kiểm tra tính hợp lệ của cài đặt
        /// </summary>
        /// <returns>True nếu cài đặt hợp lệ</returns>
        public bool IsValid()
        {
            return DeviceThreshold > 0 && 
                   DeviceThreshold <= 1000 && 
                   ScanInterval >= 30 && 
                   ScanInterval <= 3600;
        }

        /// <summary>
        /// Khôi phục giá trị mặc định theo code gốc của bạn
        /// </summary>
        public void ResetToDefaults()
        {
            StartWithWindows = true;
            NotifyNewDevice = false;
            NotifyDisconnect = true;
            NotifyUnknownMAC = true;
            DeviceThreshold = 50;
            ScanInterval = 300;
            LastUpdated = DateTime.Now;
        }

        /// <summary>
        /// Tạo bản sao của cài đặt hiện tại
        /// </summary>
        public SettingsData Clone()
        {
            return new SettingsData
            {
                StartWithWindows = this.StartWithWindows,
                NotifyNewDevice = this.NotifyNewDevice,
                NotifyDisconnect = this.NotifyDisconnect,
                NotifyUnknownMAC = this.NotifyUnknownMAC,
                DeviceThreshold = this.DeviceThreshold,
                ScanInterval = this.ScanInterval,
                CreatedAt = this.CreatedAt,
                LastUpdated = DateTime.Now
            };
        }

        /// <summary>
        /// So sánh hai cài đặt có giống nhau không
        /// </summary>
        public bool Equals(SettingsData other)
        {
            if (other == null) return false;
            
            return StartWithWindows == other.StartWithWindows &&
                   NotifyNewDevice == other.NotifyNewDevice &&
                   NotifyDisconnect == other.NotifyDisconnect &&
                   NotifyUnknownMAC == other.NotifyUnknownMAC &&
                   DeviceThreshold == other.DeviceThreshold &&
                   ScanInterval == other.ScanInterval;
        }
    }
}