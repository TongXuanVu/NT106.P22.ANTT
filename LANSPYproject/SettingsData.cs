namespace LANSPYproject
{
    public class SettingsData
    {
        public bool StartWithWindows { get; set; } = true;
        public bool NotifyNewDevice { get; set; } = false;
        public bool NotifyDisconnect { get; set; } = true;
        public bool NotifyUnknownMAC { get; set; } = true;
        public int DeviceThreshold { get; set; } = 50;
        public int ScanInterval { get; set; } = 300;
    }
}
