namespace LANSPYproject
{
    public class Device
    {
        public int ID { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }
        public string Name { get; set; }
        public string Date { get; set; } = System.DateTime.Now.ToString("dd/MM, hh:mm tt");
    }
}
