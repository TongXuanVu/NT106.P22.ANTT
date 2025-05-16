namespace LANSPYproject
{
    // Lớp mô tả thiết bị kết nối mạng
    public class NetworkConnectionDevice
    {
        public string IP { get; set; }  // Địa chỉ IP của thiết bị
        public string MAC { get; set; }  // Địa chỉ MAC của thiết bị
        public string HostName { get; set; }  // Tên máy chủ của thiết bị
        public bool IsOnline { get; set; }  // Trạng thái online của thiết bị
        public bool IsStrangeDevice { get; set; } // Thiết bị lạ (cảnh báo)
    }
}
